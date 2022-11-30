import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AuthGuard } from './services/auth.service';

import { HttpClientModule } from "@angular/common/http";
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './layouts/header/header.component';
import { FooterComponent } from './layouts/footer/footer.component';
import { SidebarComponent } from './layouts/sidebar/sidebar.component';
import { DashboardComponent } from './examples/dashboard/dashboard.component';
import { AlertsComponent } from './components/alerts/alerts.component';
import { AccordionComponent } from './components/accordion/accordion.component';
import { BadgesComponent } from './components/badges/badges.component';
import { BreadcrumbsComponent } from './components/breadcrumbs/breadcrumbs.component';
import { ButtonsComponent } from './components/buttons/buttons.component';
import { CardsComponent } from './components/cards/cards.component';
import { CarouselComponent } from './components/carousel/carousel.component';
import { ListGroupComponent } from './components/list-group/list-group.component';
import { ModalComponent } from './components/modal/modal.component';
import { TabsComponent } from './components/tabs/tabs.component';
import { PaginationComponent } from './components/pagination/pagination.component';
import { ProgressComponent } from './components/progress/progress.component';
import { SpinnersComponent } from './components/spinners/spinners.component';
import { TooltipsComponent } from './components/tooltips/tooltips.component';
import { FormsElementsComponent } from './components/forms-elements/forms-elements.component';
import { FormsLayoutsComponent } from './components/forms-layouts/forms-layouts.component';
import { FormsEditorsComponent } from './components/forms-editors/forms-editors.component';
import { TablesGeneralComponent } from './components/tables-general/tables-general.component';
import { TablesDataComponent } from './components/tables-data/tables-data.component';
import { ChartsChartjsComponent } from './components/charts-chartjs/charts-chartjs.component';
import { ChartsApexchartsComponent } from './components/charts-apexcharts/charts-apexcharts.component';
import { IconsBootstrapComponent } from './components/icons-bootstrap/icons-bootstrap.component';
import { IconsRemixComponent } from './components/icons-remix/icons-remix.component';
import { IconsBoxiconsComponent } from './components/icons-boxicons/icons-boxicons.component';
import { UsersProfileComponent } from './examples/users-profile/users-profile.component';
import { PagesFaqComponent } from './examples/pages-faq/pages-faq.component';
import { PagesContactComponent } from './examples/pages-contact/pages-contact.component';
import { PagesRegisterComponent } from './examples/pages-register/pages-register.component';
import { PagesError404Component } from './examples/pages-error404/pages-error404.component';
import { PagesBlankComponent } from './examples/pages-blank/pages-blank.component';

import { LoginComponent } from './pages/login/login.component';
import { ChecksComponent } from './pages/checks/checks.component';
import { ListEmployeesComponent } from './pages/list-employees/list-employees.component';
import { DevicesComponent } from './pages/devices/devices.component';
import { ModalDeviceComponent } from './custom-components/modals/modal-device/modal-device.component';
import { ModalDevicesComponent } from './custom-components/modals/modal-devices/modal-devices.component';
import { HomeComponent } from './pages/home/home.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ModalEmployeeComponent } from 'src/app/custom-components/modals/modal-employee/modal-employee.component';
import { DocFlowComponent } from './pages/doc-flow/doc-flow.component';
import { ModalAppruvedComponent } from './custom-components/modals/modal-appruved/modal-appruved.component';
import { ModalNewFlowComponent } from './custom-components/modals/modal-new-flow/modal-new-flow.component';
import { PdfViewerModule } from 'ng2-pdf-viewer';
import { DocumentComponent } from './pages/document/document.component';
import { DocumentDetailComponent } from './custom-components/forms/document-detail.component';
import { ItemComponent } from './custom-components/badgets/badget-item/item.component';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    FooterComponent,
    SidebarComponent,
    DashboardComponent,
    AlertsComponent,
    AccordionComponent,
    BadgesComponent,
    BreadcrumbsComponent,
    ButtonsComponent,
    CardsComponent,
    CarouselComponent,
    ListGroupComponent,
    ModalComponent,
    TabsComponent,
    PaginationComponent,
    ProgressComponent,
    SpinnersComponent,
    TooltipsComponent,
    FormsElementsComponent,
    FormsLayoutsComponent,
    FormsEditorsComponent,
    TablesGeneralComponent,
    TablesDataComponent,
    ChartsChartjsComponent,
    ChartsApexchartsComponent,
    IconsBootstrapComponent,
    IconsRemixComponent,
    IconsBoxiconsComponent,
    UsersProfileComponent,
    PagesFaqComponent,
    PagesContactComponent,
    PagesRegisterComponent,

    ModalDeviceComponent,
    ModalDevicesComponent,
    LoginComponent,
    PagesError404Component,
    PagesBlankComponent,
    ChecksComponent,
    ListEmployeesComponent,
    DevicesComponent,
    ModalEmployeeComponent,
    DocFlowComponent,
    ModalAppruvedComponent,
    ModalNewFlowComponent,
    HomeComponent,
    DocumentComponent,
    DocumentDetailComponent,
    ItemComponent
  ],
  imports: [
    HttpClientModule,
    ReactiveFormsModule,
    BrowserModule,
    AppRoutingModule,
    PdfViewerModule
  ],
  providers: [AuthGuard],
  bootstrap: [AppComponent]
})
export class AppModule { }
