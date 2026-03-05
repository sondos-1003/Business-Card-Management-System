import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { BusinessCard, BusinessCardFilterDto, CreateBusinessCardDto, UpdateBusinessCardDto } from '../models/business-card.model';

@Injectable({
  providedIn: 'root'
})
export class BusinessCardService {

  // Backend URL (use https for local .NET dev if HTTPS is enabled)
  apiUrl = 'https://localhost:7165/api/BusinessCards/';

  constructor(private http: HttpClient) {}

  // Get all cards with pagination
  getCards(page: number = 1, pageSize: number = 5): Observable<{ data: BusinessCard[], totalCount: number, page: number, pageSize: number }> {
    let params = new HttpParams()
      .set('page', page.toString())
      .set('pageSize', pageSize.toString());

    return this.http.get<{ data: BusinessCard[], totalCount: number, page: number, pageSize: number }>(this.apiUrl, { params });
  }

  // Filter cards using /filter endpoint
  filterCards(filter: BusinessCardFilterDto): Observable<BusinessCard[]> {
    const url = this.buildUrl('filter');
    return this.http.get<BusinessCard[]>(url, { params: this.buildHttpParams(filter) });
  }

  // Convert filter object to HttpParams for GET request
 private buildHttpParams(filter: BusinessCardFilterDto): HttpParams {
  let params = new HttpParams();

  if (filter.name) params = params.set('name', filter.name);
  if (filter.email) params = params.set('email', filter.email);
  if (filter.phoneNumber) params = params.set('phoneNumber', filter.phoneNumber);
  if (filter.gender) params = params.set('gender', filter.gender);
  if (filter.dob) params = params.set('dob', filter.dob); // just use string

  return params;
}

  // Add a new business card
  addCard(card: CreateBusinessCardDto): Observable<BusinessCard> {
    return this.http.post<BusinessCard>(this.apiUrl, card);
  }

  // Update an existing business card
  updateCard(id: number, card: UpdateBusinessCardDto): Observable<BusinessCard> {
    const url = this.buildUrl(String(id));
    return this.http.put<BusinessCard>(url, card);
  }

  // Delete a card by ID
  deleteCard(id: number): Observable<void> {
    const url = this.buildUrl(String(id));
    return this.http.delete<void>(url);
  }

  // Helper to join apiUrl and path without double slashes
  private buildUrl(path: string = ''): string {
    if (!path) return this.apiUrl;
    const a = this.apiUrl.endsWith('/') ? this.apiUrl.slice(0, -1) : this.apiUrl;
    const b = path.startsWith('/') ? path.slice(1) : path;
    return `${a}/${b}`;
  }
}