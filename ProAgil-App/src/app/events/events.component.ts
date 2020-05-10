import { EventService } from './../services/event/event.service';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {
  eventsFiltered: any = [];
  events: any = [];
  showImg = false;
  // tslint:disable-next-line: variable-name
  _filterList: string;
  modalRef: BsModalRef;
  constructor(private eventService: EventService, private modalService: BsModalService) { }
  ngOnInit() {
    this.getEvents();
  }
  get filterList(): string {
    return this._filterList;
  }
  set filterList(value: string) {
    this._filterList = value;
    this.eventsFiltered = this._filterList ? this.filterEvent(this._filterList) : this.events;
  }
  // Abrir modal
  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }
  filterEvent(filterBy: string): any {
    filterBy = filterBy.toLocaleLowerCase();
    return this.events.filter(
      (      event: { theme: string; }) => event.theme.toLocaleLowerCase().indexOf(filterBy) !== -1
    );
  }
  // mostrar | ocultar imagem
  alterImg() {
    this.showImg = !this.showImg;
  }
  // Obter eventos
  getEvents() {
    this.eventService.getEvents().subscribe(
      response => {
        this.events = response;
        this.eventsFiltered = this.events;
      }, error => {
        console.log(error);
      }
    );
  }
}
