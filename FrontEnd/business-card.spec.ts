import { TestBed } from '@angular/core/testing';

import { BusinessCard } from './business-card';

describe('BusinessCard', () => {
  let service: BusinessCard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(BusinessCard);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
