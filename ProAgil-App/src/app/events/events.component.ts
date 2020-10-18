import { Events } from './../models/Events';
import { EventService } from './../services/event/event.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ModalDirective } from 'ngx-bootstrap/modal/public_api';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {
  title = 'Eventos';
  file: File;
  eventsFiltered: any = [];
  events: any = [];
  event: Events;
  showImg = false;
  // tslint:disable-next-line: variable-name
  _filterList: string;
  registerForm: FormGroup;
  bodyDeletarEvento: string;

  constructor(private eventService: EventService, private fb: FormBuilder, private toastr: ToastrService) { }
  ngOnInit() {
    this.validation();
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
  openModal(template: ModalDirective) {
    this.registerForm.reset();
    template.show();
  }

  newEvent(template: any) {
    this.event = null;
    this.openModal(template);
  }
  editEvent(template: ModalDirective, event: Events) {
    this.openModal(template);
    this.event = Object.assign({}, event);
    this.event.imageURL = '';
    this.registerForm.patchValue(this.event);
  }

  deleteEvent(confirm: ModalDirective, event: Events) {
    this.openModal(confirm);
    this.event = event;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${event.theme}, Código: ${event.id}`;
}

  confirmDelete(confirm: ModalDirective) {
    this.eventService.deleteEvent(this.event.id).subscribe(
      () => {
        confirm.hide();
        this.getEvents();
        this.toastr.success(`Evento ${this.event.theme} exluído com sucesso`, 'Exluir');
      }, error => {
        console.log(error);
        this.toastr.error(`Não é possível excluir evento ${this.event.theme}: ${error}`, 'Exluir');
      }
    );
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
        this.toastr.error(`Erro ao obter eventos: ${error}`);
      }
    );
  }

  validation() {
    this.registerForm = this.fb.group({
      theme: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      eventDate: ['', Validators.required],
      personQtd: ['', [Validators.required, Validators.max(120000), Validators.pattern(/^-?(0|[1-9]\d*)?$/)]],
      imageURL: ['', Validators.required],
      contactPhone: ['', Validators.required],
      contactEmail: ['', [Validators.required, Validators.email]]
    });
  }
  // Salvar/editar evento
  saveEditions(template: ModalDirective) {
    if (this.registerForm.valid) {
      // copiar evento
      if (!this.event){
        this.event = Object.assign(this.registerForm.value);
        this.splitImage();
      }else {
        this.event = Object.assign({ id: this.event.id }, this.registerForm.value);
        this.splitImage();
      }
      this.eventService.saveEvent(this.event).subscribe(
        (newEvent: Events) => {
          template.hide();
          this.getEvents();
          this.toastr.success(`Evento ${this.event.theme} salvo com sucesso`, 'Salvar');
        }, error => {
          console.log(error);
          this.toastr.error(`Não foi possível salvar evento ${this.event.theme}: ${error}`, 'Salvar');
        }
      );
    }
  }

  splitImage(){
    const fileName = this.event.imageURL.split('\\', 3);
    this.event.imageURL = fileName[2];

    this.eventService.postUpload(this.file, fileName[2]).subscribe(); // upload de imagem
  }

  onFileChange(event: any) {
    const reader = new FileReader();
    if (event.target.files && event.target.files.length) {
      this.file = event.target.files;
      console.log(event);
    }
  }
}
