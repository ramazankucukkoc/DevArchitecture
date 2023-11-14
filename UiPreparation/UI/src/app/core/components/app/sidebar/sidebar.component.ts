import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { AuthService } from '../../admin/login/Services/Auth.service';


declare const $: any;
declare interface RouteInfo {
    path: string;
    title: string;
    icon: string;
    class: string;
    claim:string;
    claim1:string;
    claim2:string;
}
export const ADMINROUTES: RouteInfo[] = [
  { path: '/user', title: 'Users', icon: 'how_to_reg', class: '', claim:"Yonetici",claim1:null,claim2:null },
  { path: '/group', title: 'Groups', icon:'groups', class: '',claim:"Yonetici",claim1:null,claim2:null },
  { path: '/operationclaim', title: 'OperationClaim', icon: 'local_police', class: '', claim:"Yonetici",claim1:null,claim2:null},
  { path: '/language', title: 'Languages', icon:'language', class: '', claim:"Yonetici",claim1:null,claim2:null },
  { path: '/translate', title: 'TranslateWords', icon: 'translate', class: '', claim: "Yonetici" ,claim1:null,claim2:null},
  { path: '/log', title: 'Logs', icon: 'update', class: '', claim: "Yonetici" ,claim1:null,claim2:null},
  { path: '/customer', title: 'Customers', icon: '', class: '', claim: "Yonetici",claim1:"MusteriTemsilcisi",claim2:null },
  { path: '/product', title: 'Products', icon: '', class: '', claim: "Yonetici" ,claim1:"MusteriTemsilcisi",claim2:"Kullanici"},
  { path: '/orderinformation', title: 'OrderInformations', icon: '', class: '', claim: "Yonetici" ,claim1:"MusteriTemsilcisi",claim2:"Kullanici"},
  { path: '/warehouseinformation', title: 'WarehouseInformations', icon: '', class: '', claim: "Yonetici",claim1:"MusteriTemsilcisi",claim2:"Kullanici" }

//https://localhost:5001/api/v1/WarehouseInformations

];

export const USERROUTES: RouteInfo[] = [ 
  //{ path: '/log', title: 'Logs', icon: 'update', class: '', claim: "GetLogDtoQuery" }
];

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit {
  adminMenuItems: any[];
  userMenuItems: any[];

  constructor(private router:Router, private authService:AuthService,public translateService:TranslateService) {
    
  }

  ngOnInit() {
  
    this.adminMenuItems = ADMINROUTES.filter(menuItem => menuItem);
    this.userMenuItems = USERROUTES.filter(menuItem => menuItem);

    var lang=localStorage.getItem('lang') || 'tr-TR'
    this.translateService.use(lang);
  }
  isMobileMenu() {
      if ($(window).width() > 991) {
          return false;
      }
      return true;
  };

  checkClaim(claim:string):boolean{
    return this.authService.claimGuard(claim)
  }
  ngOnDestroy() {
    if (!this.authService.loggedIn()) {
      this.authService.logOut();
      this.router.navigateByUrl("/login");
    }
  } 
 }

