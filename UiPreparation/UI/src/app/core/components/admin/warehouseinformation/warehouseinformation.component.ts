import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from "@angular/core";import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { WarehouseInformation } from "./models/WarehouseInformation";
import { AuthService } from "../login/Services/Auth.service";
import { FormBuilder, FormGroup } from "@angular/forms";
import { AlertifyService } from "app/core/services/Alertify.service";
import { WarehouseInformationService } from "./Services/WarehouseInformationService";
declare var jQuery: any;

@Component({
  selector: 'app-warehouseinformation',
  templateUrl: './warehouseinformation.component.html',
  styleUrls: ['./warehouseinformation.component.css']
})
export class WarehouseinformationComponent implements OnInit {

  dataSource: MatTableDataSource<any>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = [
    "id",
    "productId",
    "productName",
    "count",
    "readyForSale",
    "status",
    "update",
    "delete",
  ];

  warehouseInformation: WarehouseInformation;
  warehouseInformationList: WarehouseInformation[];
  id: number;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private warehouseInformationService: WarehouseInformationService,
    private alertifyService: AlertifyService

  ) { }

  ngAfterViewInit(): void {
    this.getwarehouseInformationList();
  }
  warehouseInformationAddForm: FormGroup;

  ngOnInit(): void {
    this.createwarehouseInformationAddForm();

  }
  checkClaim(claim: string): boolean {
    return this.authService.claimGuard(claim);

  }
  createwarehouseInformationAddForm() {
    this.warehouseInformationAddForm = this.formBuilder.group({
      id: [0],
      productId: [""],
      count: [""],
      status:[true],
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
    if (this.warehouseInformationAddForm.valid) {
      this.warehouseInformation = Object.assign({}, this.warehouseInformationAddForm.value);

      if (this.warehouseInformation.id == 0) this.addwarehouseInformation();
      else this.updatewarehouseInformation();
    }
  }
  
  setWarehouseInformationId(id: number) {
    this.id = id;
  }
  addwarehouseInformation() {
    
    this.warehouseInformationService.addwarehouseInformation(this.warehouseInformation).subscribe((
      data) => {    
      this.getwarehouseInformationList();
      this.warehouseInformation = new WarehouseInformation();
      jQuery("#warehouseInformation").modal("hide");
      this.alertifyService.success(data);
      this.clearFormGroup(this.warehouseInformationAddForm);
    },
    (error) => {
     // jQuery("#warehouseInformation").modal("hide");
      this.alertifyService.error(error.error);
    });
  }
  updatewarehouseInformation() {
    this.warehouseInformationService.updatewarehouseInformation(this.warehouseInformation).subscribe((data) => {
      var index = this.warehouseInformationList.findIndex((x) => x.id == this.warehouseInformation.id);
      this.warehouseInformationList[index] = this.warehouseInformation;
      this.dataSource = new MatTableDataSource(this.warehouseInformationList);
      this.configDataTable();
      this.warehouseInformation = new WarehouseInformation();
      jQuery("#warehouseInformation").modal("hide");
      this.alertifyService.success(data);
      this.clearFormGroup(this.warehouseInformationAddForm);
    },(error) => {
    //  jQuery("#warehouseInformation").modal("hide");
      this.alertifyService.error(error.error);
    });
  }
  configDataTable(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  getwarehouseInformationList() {
      this.warehouseInformationService.getwarehouseInformationList().subscribe((data) => {
      this.warehouseInformationList = data;
      this.dataSource = new MatTableDataSource(data);
      this.configDataTable();
    });
  }
  deletewarehouseInformation(id: number) {
    this.warehouseInformationService.deletewarehouseInformation(id).subscribe((data) => {
      this.alertifyService.success(data.toString());
      var index = this.warehouseInformationList.findIndex((x) => x.id == id);
      this.warehouseInformationList[index].status = false;
      this.dataSource = new MatTableDataSource(this.warehouseInformationList);
      this.configDataTable();
    });
  }
  getwarehouseInformationById(id: number) {
    this.clearFormGroup(this.warehouseInformationAddForm);
    this.warehouseInformationService.getwarehouseInformationById(id).subscribe((data) => {
      this.warehouseInformation = data;
      this.warehouseInformationAddForm.patchValue(data);
    });
  }
}
