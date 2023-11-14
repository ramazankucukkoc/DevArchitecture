import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from "@angular/core";import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { AuthService } from "../login/Services/Auth.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AlertifyService } from "app/core/services/Alertify.service";
import { Color } from "./models/color";
import { ColorService } from "./Services/ColorService";

  declare var jQuery: any;

@Component({
  selector: 'app-color',
  templateUrl: './color.component.html',
  styleUrls: ['./color.component.css']
})

export class ColorComponent implements OnInit {
  dataSource: MatTableDataSource<any>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = [
    "id",
    "name",
    "status",
    "lastUpdatedDate",
    "update",
    "delete",
  ];

  color: Color;
  colorList: Color[];
  id: number;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private colorService: ColorService,
    private alertifyService: AlertifyService

  ) { }

  ngAfterViewInit(): void {
    this.getColorList();
    
  }
  colorAddForm: FormGroup;

  ngOnInit(): void {
    this.createColorAddForm();

  }
  checkClaim(claim: string): boolean {
    return this.authService.claimGuard(claim);

  }
  createColorAddForm() {
    this.colorAddForm = this.formBuilder.group({
      id: [0],
      name: ["", Validators.required],
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
    if (this.colorAddForm.valid) {
      this.color = Object.assign({}, this.colorAddForm.value);

      if (this.color.id == 0) this.addColor();
      else this.updateProduct();
    }
  }
  
  setProductId(id: number) {
    this.id = id;
  }
  addColor() {
    this.colorService.addColor(this.color).subscribe((data) => {
      this.getColorList();
      this.color = new Color();
      jQuery("#color").modal("hide");
      this.alertifyService.success(data);
      this.clearFormGroup(this.colorAddForm);
    },
    (error) => {
      //jQuery("#product").modal("hide");
      this.alertifyService.error(error.error);
    });
  }
  updateProduct() {
    this.colorService.updateColor(this.color).subscribe((data) => {
      var index = this.colorList.findIndex((x) => x.id == this.color.id);
      this.colorList[index] = this.color;
      this.dataSource = new MatTableDataSource(this.colorList);
      this.configDataTable();
      this.color = new Color();
      jQuery("#color").modal("hide");
      this.alertifyService.success(data);
      this.clearFormGroup(this.colorAddForm);
    },
    (error) => {
     // jQuery("#product").modal("hide");
      this.alertifyService.error(error.error);
    });
  }
  configDataTable(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }
  getColorList() {
    this.colorService.getColorList().subscribe((data) => {
      this.colorList = data;
      this.dataSource = new MatTableDataSource(data);
      this.configDataTable();
    });

  }
  deleteColor(id: number) {
    this.colorService.deleteColor(id).subscribe((data) => {
      this.alertifyService.success(data.toString());
      var index = this.colorList.findIndex((x) => x.id == id);
      this.colorList[index].status = false;
      this.dataSource = new MatTableDataSource(this.colorList);
      this.configDataTable();
    });
  }
  getColorById(id: number) {
    this.clearFormGroup(this.colorAddForm);
    this.colorService.getColorById(id).subscribe((data) => {
      this.color = data;
      this.colorAddForm.patchValue(data);
    });
  }
}
