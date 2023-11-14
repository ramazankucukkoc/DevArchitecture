/* tslint:disable:no-unused-variable */

import { TestBed, inject } from '@angular/core/testing';
import { CustomerService } from './CustomerService';

describe('Service: Customer', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CustomerService]
    });
  });

  it('should ...', inject([CustomerService], (service: CustomerService) => {
    expect(service).toBeTruthy();
  }));
});
