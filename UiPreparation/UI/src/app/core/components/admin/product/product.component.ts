import {
  AfterViewInit,
  Component,
  ElementRef,
  OnInit,
  ViewChild,
} from "@angular/core";import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from "@angular/material/paginator";
import { MatSort } from "@angular/material/sort";
import { Product } from "./models/product";
import { AuthService } from "../login/Services/Auth.service";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { AlertifyService } from "app/core/services/Alertify.service";
import { ProductService } from "./Services/ProductService";
import { IDropdownSettings } from "ng-multiselect-dropdown";
import { environment } from "environments/environment";
import { LookUpService } from "app/core/services/LookUp.service";
import { LookUp } from "app/core/models/LookUp";

  declare var jQuery: any;

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})

export class ProductComponent implements OnInit {
  dataSource: MatTableDataSource<any>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = [
    "id",
    "name",
    "colorId",
    "size",
    "status",
    "lastUpdatedDate",
    "update",
    "delete",
    "updateColor",

  ];
  product: Product;
  productList: Product[];
  colorDropdownList: LookUp[];
  colorSelectedItems: LookUp[];

dropdownSettings: IDropdownSettings;
isGroupChange: boolean = false;
isColorChange: boolean = false;
id: number;

  constructor(
    private authService: AuthService,
    private formBuilder: FormBuilder,
    private productService: ProductService,
    private alertifyService: AlertifyService,
    private lookUpService: LookUpService,

  ) { }

  ngAfterViewInit(): void {
    this.getProductList();
    
  }
  productAddForm: FormGroup;

  ngOnInit() {
    this.createProductAddForm();
    this.dropdownSettings = environment.getDropDownSetting;

    this.lookUpService.getColorLookUp().subscribe((data) => {
      this.colorDropdownList = data;
    });

  }
  checkClaim(claim: string): boolean {
    return this.authService.claimGuard(claim);

  }
  createProductAddForm() {
    this.productAddForm = this.formBuilder.group({
      id: [0],
      name: ["", Validators.required],
      colorId: [""],
      size: [""],
      status:[true],
    });
  }
  sizes: string[] = ['S', 'M'];

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
    if (this.productAddForm.valid) {
      this.product = Object.assign({}, this.productAddForm.value);

      if (this.product.id == 0) this.addProduct();
      else this.updateProduct();
    }
  }
  
  setProductId(id: number) {
    this.id = id;
  }
  addProduct() {
    this.productService.addProduct(this.product).subscribe((data) => {
      this.getProductList();
      this.product = new Product();
      jQuery("#product").modal("hide");
      this.alertifyService.success(data);
      this.clearFormGroup(this.productAddForm);
    },
    (error) => {
      //jQuery("#product").modal("hide");
      this.alertifyService.error(error.error);
    });
  }
  saveProductColorsPermission() {
    if (this.isColorChange) {
      var ids = this.colorSelectedItems.map(function (x) {
        return x.id as number;
      });
      this.productService.saveProductColors(this.id, ids).subscribe(
        (data) => {
          jQuery("#colorsPermissions").modal("hide");
          this.isColorChange = false;
          this.alertifyService.success(data);
          this.getProductList();
        },
        (error) => {
          this.alertifyService.error(error.error);
          jQuery("#colorsPermissions").modal("hide");
        }
      );
    }
  }
  updateProduct() {
    this.productService.updateProduct(this.product).subscribe((data) => {
      var index = this.productList.findIndex((x) => x.id == this.product.id);
      this.productList[index] = this.product;
      this.dataSource = new MatTableDataSource(this.productList);
      this.configDataTable();
      this.product = new Product();
      jQuery("#product").modal("hide");
      this.alertifyService.success(data);
      this.clearFormGroup(this.productAddForm);
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
  getProductList() {
    this.productService.getProductList().subscribe((data) => {
      this.productList = data;
      this.dataSource = new MatTableDataSource(data);
      this.configDataTable();
    });

  }
  deleteProduct(id: number) {
    this.productService.deleteProduct(id).subscribe((data) => {
      this.alertifyService.success(data.toString());
      var index = this.productList.findIndex((x) => x.id == id);
      this.productList[index].status = false;
      this.dataSource = new MatTableDataSource(this.productList);
      this.configDataTable();
    });
  }
  getProductById(id: number) {
    this.clearFormGroup(this.productAddForm);
    this.productService.getProductById(id).subscribe((data) => {
      this.product = data;
      this.productAddForm.patchValue(data);
    });
  } 
  getProductColorsPermissions(id: number) {
    this.id = id;
    this.productService.getProductColors(id).subscribe((data) => {
      this.colorSelectedItems = data;
    });
  }
  onItemSelect(comboType: string) {
    this.setComboStatus(comboType);
  } 
  setComboStatus(comboType: string) {
    if (comboType == "Group") this.isGroupChange = true;
    else if (comboType == "Color") this.isColorChange = true;
  }
  onSelectAll(comboType: string) {
    this.setComboStatus(comboType);
  }
  onItemDeSelect(comboType: string) {
    this.setComboStatus(comboType);
  }
 
}
