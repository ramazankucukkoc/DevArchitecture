/* tslint:disable:no-unused-variable */

import { TestBed, inject } from '@angular/core/testing';
import { WarehouseInformationService } from './WarehouseInformationService';

describe('Service: Customer', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [WarehouseInformationService]
    });
  });

  it('should ...', inject([WarehouseInformationService], (service: WarehouseInformationService) => {
    expect(service).toBeTruthy();
  }));
});
