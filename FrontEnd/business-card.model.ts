export interface BusinessCard {
  id?: number;
  name: string;
  gender: string;
  dob: string; // ISO string e.g. "2026-02-21"
  email: string;
  phoneNumber: string;
  address: string;
}

export interface CreateBusinessCardDto {
  name: string;
  gender: string;
  dob: string; // ISO string e.g. "2026-02-04"
  email: string;
  phoneNumber: string;
  address: string;
}

export interface UpdateBusinessCardDto {
  name: string;
  gender: string;
  dob: string;
  email: string;
  phoneNumber: string;
  address: string;
}

export interface BusinessCardFilterDto {
  name?: string;
  email?: string;
  phoneNumber?: string;
  gender?: string;
  dob?: string; // optional filter
}