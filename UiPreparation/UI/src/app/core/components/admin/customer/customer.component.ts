import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from "@angular/core";
import { AuthService } from '../login/Services/Auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatTableDataSource } from "@angular/material/table";
import { Customer } from './models/Customer';
import { CustomerService } from './Services/CustomerService';
import { AlertifyService } from 'app/core/services/Alertify.service';
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";

declare var jQuery: any;

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.scss']
})
export class CustomerComponent implements OnInit {
  dataSource: MatTableDataSource<any>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = [
    "id",
    "email",
    "name",
    "status",
    "mobilePhones",
    "address",
    "code",
    "update",
    "delete",
  ];

  customer: Customer;
  customerList: Customer[];
  id: number;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private customerService: CustomerService,
    private alertifyService: AlertifyService

  ) { }

  ngAfterViewInit(): void {
    this.getCustomerList();
  }
  customerAddForm: FormGroup;

  ngOnInit(): void {
    this.createCustomerAddForm();

  }
  checkClaim(claim: string): boolean {
    return this.authService.claimGuard(claim);

  }
  createCustomerAddForm() {
    this.customerAddForm = this.formBuilder.group({
      id: [0],
      name: ["", Validators.required],
      code: [""],
      mobilePhones: [""],
      address: [""],
      email: ["", Validators.required],
      status: [true],
    });
  }
  clearFormGroup(group: FormGroup) {
    group.markAsUntouched();
    group.reset();

    Object.keys(group.controls).forEach((key) => {
      group.get(key).setErrors(null);
      if (key == "id") group.get(key).setValue(0);
      else if (key == "status") group.get(key).setValue(true);
    });
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }
  save() {
    if (this.customerAddForm.valid) {
      this.customer = Object.assign({}, this.customerAddForm.value);

      if (this.customer.id == 0) this.addCustomer();
      else this.updateCustomer();
    }
  }
  
  setCustomerId(id: number) {
    this.id = id;
  }
  addCustomer() {
    this.customerService.addCustomer(this.customer).subscribe((data) => {
      this.getCustomerList();
      this.customer = new Customer();
      jQuery("#customer").modal("hide");
      this.alertifyService.success(data);
      this.clearFormGroup(this.customerAddForm);
    },
    (error) => {
      //jQuery("#customer").modal("hide");
      this.alertifyService.error(error.error);
    });
  }
  updateCustomer() {
    this.customerService.updateCustomer(this.customer).subscribe((data) => {
      var index = this.customerList.findIndex((x) => x.id == this.customer.id);
      this.customerList[index] = this.customer;
      this.dataSource = new MatTableDataSource(this.customerList);
      this.configDataTable();
      this.customer = new Customer();
      jQuery("#customer").modal("hide");
      this.alertifyService.success(data);
      this.clearFormGroup(this.customerAddForm);
    },
    (error) => {
      this.alertifyService.error(error.error);
    });
  }
  configDataTable(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  getCustomerList() {
    this.customerService.getCustomerList().subscribe((data) => {
      this.customerList = data;
      this.dataSource = new MatTableDataSource(data);
      this.configDataTable();
    });
  }
  deleteCustomer(id: number) {
    this.customerService.deleteCustomer(id).subscribe((data) => {
      this.alertifyService.success(data.toString());
      var index = this.customerList.findIndex((x) => x.id == id);
      this.customerList[index].status = false;
      this.dataSource = new MatTableDataSource(this.customerList);
      this.configDataTable();
    });
  }
  getCustomerById(id: number) {
    this.clearFormGroup(this.customerAddForm);
    this.customerService.getCustomerById(id).subscribe((data) => {
      this.customer = data;
      this.customerAddForm.patchValue(data);
    });
  }
}
