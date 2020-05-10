/** MODULES */
import { AppRoutingModule } from './app-routing.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { TooltipModule } from 'ngx-bootstrap/tooltip';

/** SERVICES */
import { EventService } from './services/event/event.service';

/** COMPONENTS */
import { AppComponent } from './app.component';
import { EventsComponent } from './events/events.component';
import { NavComponent } from './nav/nav.component';

/** PIPES */
import { DateTimeFormatPipe } from './helpers/DateTimeFormat.pipe';

@NgModule({
   declarations: [
      AppComponent,
      EventsComponent,
      NavComponent,
      DateTimeFormatPipe
   ],
   imports: [
      BrowserModule,
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      BrowserAnimationsModule,
      TooltipModule.forRoot(), // forRoot() para utilizar em toda estrutura do projeto
      ModalModule.forRoot(),
      BsDropdownModule.forRoot()
   ],
   providers: [EventService],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
