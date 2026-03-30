import { Component, OnInit, SecurityContext } from '@angular/core';
import { FormBuilder, Validators, FormGroup, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';
import { BusinessCardService } from '../../services/business-card.service';
import { BusinessCard, BusinessCardFilterDto, CreateBusinessCardDto } from '../../models/business-card.model';

@Component({
  selector: 'app-add-card',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, FormsModule],
  templateUrl: './add-card.component.html',
  styleUrls: ['./add-card.css']
})
export class AddCardComponent implements OnInit {

  form!: FormGroup;
  previewData: BusinessCard | null = null;
  editingId: number | null = null;

  // typed filters
  filters: BusinessCardFilterDto = { name: '', email: '', phoneNumber: '', gender: '', dob: '' };
totalItems: number = 0; // declared in component
currentPage: number = 1;
pageSize: number = 5;

  cards: BusinessCard[] = [];   // typed array
       // declare totalItems to avoid TS error

  // Modal state for filtered results
  showFilterModal: boolean = false;
  filteredResults: BusinessCard[] = [];
  hasFiltersApplied: boolean = false;

  // Modal state for all records
  showAllRecordsModal: boolean = false;

  constructor(private fb: FormBuilder, private service: BusinessCardService, private sanitizer: DomSanitizer) {}

  ngOnInit(): void {
    this.form = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2), Validators.maxLength(50), Validators.pattern(/^[a-zA-Z\s]+$|^[^<>'";\-\-]*$/)]],
      gender: ['', [Validators.required, Validators.pattern(/^(Male|Female)$/i)]],
      dob: ['', Validators.required],
      email: ['', [Validators.required, Validators.email, Validators.pattern(/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$|^[^<>'";\-\-]*$/)]],
      phoneNumber: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
      address: ['', [Validators.minLength(5), Validators.pattern(/^[^<>'";\-\-]*$/)]]
    });

    this.loadCards(); // load cards initially
  }

  // Preview form data before submitting
  preview() {
    if (this.form.valid) {
      let formValue = this.form.value;
      // Sanitize for preview
      formValue = {
        ...formValue,
        name: this.sanitizer.sanitize(SecurityContext.HTML, formValue.name) || '',
        email: this.sanitizer.sanitize(SecurityContext.HTML, formValue.email) || '',
        phoneNumber: this.sanitizer.sanitize(SecurityContext.HTML, formValue.phoneNumber) || '',
        address: this.sanitizer.sanitize(SecurityContext.HTML, formValue.address) || '',
      };
      this.previewData = formValue as BusinessCard;
      console.log('Sanitized data for preview:', formValue);
      alert('Sanitized data:\n' + JSON.stringify(formValue, null, 2));
    } else {
      alert('Please fill all required fields');
    }
  }

  // Submit form to backend
  submit() {
    if (this.form.valid) {
      debugger;
      let payload = this.form.value as CreateBusinessCardDto;
      
      // Sanitize inputs to prevent XSS and SQL injection attempts
      payload = {
        ...payload,
        name: this.sanitizer.sanitize(SecurityContext.HTML, payload.name) || '',
        email: this.sanitizer.sanitize(SecurityContext.HTML, payload.email) || '',
        phoneNumber: this.sanitizer.sanitize(SecurityContext.HTML, payload.phoneNumber) || '',
        address: this.sanitizer.sanitize(SecurityContext.HTML, payload.address) || '',
        gender: payload.gender,
        dob: payload.dob
      };
      
      debugger;
      if (this.editingId) {
        debugger;
        this.service.updateCard(this.editingId, payload).subscribe({
          next: () => {
            debugger;
            alert('Updated successfully');
            this.form.reset();
            this.previewData = null;
            this.editingId = null;
            this.loadCards(this.currentPage, this.pageSize);
          },
          error: (err: any) =>{ debugger; this.showError(err)}
          
        });
      } else {
        debugger;
        this.service.addCard(payload).subscribe({
          next: () => {
            debugger;
            console.log("FORM VALUE 👉", this.form.value);
            alert('Added successfully');
            this.form.reset();
            this.previewData = null;
            this.loadCards(this.currentPage, this.pageSize); // reload cards after adding
          },
          error: (err: any) => { debugger; this.showError(err); }
        });
      }
    }
  }

  // Begin editing an existing card
  editCard(card: BusinessCard) {
    if (!card.id) return;
    this.editingId = card.id;
    this.form.patchValue({
      name: card.name,
      gender: card.gender,
      dob: card.dob,
      email: card.email,
      phoneNumber: card.phoneNumber,
      address: card.address
    });
    this.previewData = null;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  cancelEdit() {
    this.editingId = null;
    this.form.reset();
    this.previewData = null;
  }

  // Apply filters using backend API
  applyFilter() {
    this.service.filterCards(this.filters).subscribe({
      next: (data: BusinessCard[]) => {
        this.filteredResults = data;
        this.showFilterModal = true;
        this.hasFiltersApplied = true;
      },
      error: (err: any) => this.showError(err)
    });
  }

  // Load all cards from backend with pagination
  loadCards(page: number = 1, pageSize: number = 5) {
  this.service.getCards(page, pageSize).subscribe({
    next: (res: { data: BusinessCard[], totalCount: number, page: number, pageSize: number }) => {
      this.cards = res.data;
      this.totalItems = res.totalCount; // match backend property
    },
    error: (err: any) => this.showError(err)
  });
}
onPageChange(page: number) {
  this.currentPage = page;
  this.loadCards(this.currentPage, this.pageSize);
}

  // Helper to build page number array for template
  get pageNumbers(): number[] {
    const totalPages = Math.ceil((this.totalItems || 0) / this.pageSize);
    return Array.from({ length: totalPages }, (_, i) => i + 1);
  }

  // Optional: delete a card
  deleteCard(id?: number) {
    if (!id) return;
    if (confirm('Are you sure you want to delete this card?')) {
      this.service.deleteCard(id).subscribe({
        next: () => {
          alert('Deleted successfully');
          this.loadCards(this.currentPage, this.pageSize); // reload after deletion
        },
        error: (err: any) => this.showError(err)
      });
    }
  }

  // Centralized error reporter to surface status and server messages
  private showError(err: any) {
    console.error('HTTP error', err);
    const status = err?.status ?? 'No status';
    let message = 'Unknown error';

    if (err?.message) {
      message = String(err.message);
    } else if (err?.error) {
      message = typeof err.error === 'string' ? err.error : JSON.stringify(err.error);
    } else if (err) {
      try {
        message = JSON.stringify(err);
      } catch {
        message = String(err);
      }
    }

    alert(`Request failed (status: ${status})\n${message}`);
  }

  // Close the filter modal
  closeFilterModal() {
    this.showFilterModal = false;
  }

  // Clear all filters and close modal
  clearFilters() {
    this.filters = { name: '', email: '', phoneNumber: '', gender: '', dob: '' };
    this.filteredResults = [];
    this.showFilterModal = false;
    this.hasFiltersApplied = false;
    this.loadCards(1, this.pageSize);
  }

  // Show all records modal
  showAllRecords() {
    this.loadCards(1, this.pageSize);
    this.showAllRecordsModal = true;
  }

  // Close all records modal
  closeAllRecordsModal() {
    this.showAllRecordsModal = false;
  }
}