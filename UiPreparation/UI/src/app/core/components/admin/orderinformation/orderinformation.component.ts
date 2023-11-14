import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from "@angular/core";import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { AuthService } from '../login/Services/Auth.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { OrderInformationService } from './Services/OrderInformationService';
import { AlertifyService } from 'app/core/services/Alertify.service';
import { OrderInformation } from "./models/OrderInformation";
declare var jQuery: any;

@Component({
  selector: 'app-orderinformation',
  templateUrl: './orderinformation.component.html',
  styleUrls: ['./orderinformation.component.css']
})
export class OrderinformationComponent implements OnInit {

  dataSource: MatTableDataSource<any>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = [
    "id",
    "productId",
    "productName",
    "customerId",
    "customerName",
    "customerEmail",
    "customerAddress",
    "count",
    "lastUpdatedDate",
    "status",
    "update",
    "delete",
  ];

  orderInformation: OrderInformation;
  orderInformationList: OrderInformation[];
  id: number;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private orderInformationService: OrderInformationService,
    private alertifyService: AlertifyService

  ) { }

  ngAfterViewInit(): void {
    this.getorderInformationList();
  }
  orderInformationAddForm: FormGroup;

  ngOnInit(): void {
    this.createOrderInformationAddForm();

  }
  checkClaim(claim: string): boolean {
    return this.authService.claimGuard(claim);

  }
  createOrderInformationAddForm() {
    this.orderInformationAddForm = this.formBuilder.group({
      id: [0],
      productId: [""],
      customerId: [""],
      count: [""],
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
    if (this.orderInformationAddForm.valid) {
      this.orderInformation = Object.assign({}, this.orderInformationAddForm.value);

      if (this.orderInformation.id == 0) this.addOrderInformation();
      else this.updateOrderInformation();
    }
  }
  
  setCustomerId(id: number) {
    this.id = id;
  }
  addOrderInformation() {
    this.orderInformationService.addorderInformation(this.orderInformation).subscribe((data) => {
      this.getorderInformationList();
      this.orderInformation = new OrderInformation();
      jQuery("#orderInformation").modal("hide");
      this.alertifyService.success(data);
      this.clearFormGroup(this.orderInformationAddForm);
    },
    (error) => {
      jQuery("#orderInformation").modal("hide");
      this.alertifyService.error(error.error);
    });
  }
  updateOrderInformation() {
    this.orderInformationService.updateorderInformation(this.orderInformation).subscribe((data) => {
      var index = this.orderInformationList.findIndex((x) => x.id == this.orderInformation.id);
      this.orderInformationList[index] = this.orderInformation;
      this.dataSource = new MatTableDataSource(this.orderInformationList);
      this.configDataTable();
      this.orderInformation = new OrderInformation();
      jQuery("#orderInformation").modal("hide");
      this.alertifyService.success(data);
      this.clearFormGroup(this.orderInformationAddForm);
    },
    (error) => {
      jQuery("#orderInformation").modal("hide");
      this.alertifyService.error(error.error);
    });
  }
  configDataTable(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  getorderInformationList() {
    this.orderInformationService.getorderInformationList().subscribe((data) => {
      this.orderInformationList = data;
      this.dataSource = new MatTableDataSource(data);
      this.configDataTable();
    });
  }
  deleteorderInformation(id: number) {
    this.orderInformationService.deleteorderInformation(id).subscribe((data) => {
      this.alertifyService.success(data.toString());
      var index = this.orderInformationList.findIndex((x) => x.id == id);
      this.orderInformationList[index].status = false;
      this.dataSource = new MatTableDataSource(this.orderInformationList);
      this.configDataTable();
    });
  }
  getorderInformationById(id: number) {
    this.clearFormGroup(this.orderInformationAddForm);
    this.orderInformationService.getorderInformationById(id).subscribe((data) => {
      this.orderInformation = data;
      this.orderInformationAddForm.patchValue(data);
    });
  }


}
