/** MODULES */
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxMaskModule } from 'ngx-mask';
import { CurrencyMaskModule } from 'ng2-currency-mask';

/** SERVICES */
import { EventService } from './services/event/event.service';
import { AuthInterceptor } from './auth/interceptor/auth.interceptor';

/** COMPONENTS */
import { AppComponent } from './app.component';
import { EventsComponent } from './events/events.component';
import { NavComponent } from './nav/nav.component';
import { SpeakersComponent } from './speakers/speakers.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ContactsComponent } from './contacts/contacts.component';
import { ErrorComponent } from './error/error.component';
import { TitleComponent } from './shared/title/title.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { EventEditComponent } from './events/event-edit/event-edit.component';

/** PIPES */
import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';



@NgModule({
   declarations: [
      AppComponent,
      EventsComponent,
      NavComponent,
      SpeakersComponent,
      DashboardComponent,
      ContactsComponent,
      DateTimeFormatPipe,
      ErrorComponent,
      TitleComponent,
      UserComponent,
      LoginComponent,
      RegistrationComponent,
      EventEditComponent,
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      BrowserAnimationsModule,
      TooltipModule.forRoot(), // forRoot() para utilizar em toda estrutura do projeto
      ModalModule.forRoot(),
      BsDropdownModule.forRoot(),
      TabsModule.forRoot(),
      NgxMaskModule.forRoot(),
      CurrencyMaskModule,
      ToastrModule.forRoot({
         timeOut: 7000,
         positionClass: 'toast-bottom-right',
         preventDuplicates: true
      })
   ],
   providers: [
      EventService,
      {
         provide: HTTP_INTERCEPTORS,
         useClass: AuthInterceptor,
         multi: true
      }
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
