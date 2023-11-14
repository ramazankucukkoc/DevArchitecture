/* tslint:disable:no-unused-variable */

import { TestBed, inject } from '@angular/core/testing';
import { ColorService } from './ColorService';

describe('Service: Color', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ColorService]
    });
  });

  it('should ...', inject([ColorService], (service: ColorService) => {
    expect(service).toBeTruthy();
  }));
});
