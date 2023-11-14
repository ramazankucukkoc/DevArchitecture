/* tslint:disable:no-unused-variable */

import { TestBed, inject } from '@angular/core/testing';
import { OrderInformationService } from './OrderInformationService';

describe('Service: OrderInformation', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [OrderInformationService]
    });
  });

  it('should ...', inject([OrderInformationService], (service: OrderInformationService) => {
    expect(service).toBeTruthy();
  }));
});
