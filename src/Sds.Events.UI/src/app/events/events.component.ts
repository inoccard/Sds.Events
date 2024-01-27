import { Events } from './../models/Events';
import { EventService } from './../services/event/event.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { ModalDirective } from 'ngx-bootstrap/modal/public_api';
import { ToastrService } from 'ngx-toastr';
import { NgxSpinnerService } from 'ngx-spinner';
import { ErrorDetails } from '../shared/ErrorDetails';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
  styleUrls: ['./events.component.css']
})
export class EventsComponent implements OnInit {
  title = 'Eventos';
  file: FileList;
  eventsFiltered: any = [];
  events: Events[] = [];
  event: Events;
  showImg = false;
  // tslint:disable-next-line: variable-name
  _filterList: string;
  registerForm: FormGroup;
  bodyDeletarEvento: string;

  constructor(
    private eventService: EventService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    private  spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.spinner.show();

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

  /**
   * abrir modal
   * @param template
   */
  openModal(template: ModalDirective) {
    this.registerForm.reset();
    template.show();
  }

  /**
   * criar novo evento
   * @param template
   */
  newEvent(template: any) {
    this.event = null;
    this.openModal(template);
  }

  /**
   * editar
   * @param template
   * @param event
   */
  editEvent(template: ModalDirective, event: Events) {
    this.openModal(template);
    this.event = Object.assign({}, event);
    this.event.imageURL = '';
    this.registerForm.patchValue(this.event);
  }

  /**
   * excluir
   * @param confirm
   * @param event
   */
  deleteEvent(confirm: ModalDirective, event: Events) {
    this.openModal(confirm);
    this.event = event;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${event.theme}, Código: ${event.id}`;
  }

  /**
   * confirmar excluir
   * @param confirm
   */
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
      (event: { theme: string; }) => event.theme.toLocaleLowerCase().indexOf(filterBy) !== -1
    );
  }

  /**
   * mostrar | ocultar imagem
   */
  alterImg() {
    this.showImg = !this.showImg;
  }
  /**
   * obter eventos
   */
  getEvents(): void {
    this.eventService.getEvents().subscribe({
      next: (events: Events[]) => {
        this.events = events;
        this.eventsFiltered = this.events;
      },
      error: (exception: any) => {
        this.spinner.hide();
        const errorDetails: ErrorDetails = exception.error;
        this.handleErrorResponse(errorDetails);
      },
      complete: () => {
        return this.spinner.hide();
      }
    });
  }

  validation() {
    this.registerForm = this.fb.group({
      theme: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      eventDate: ['', Validators.required],
      personQtd: ['', [Validators.required, Validators.max(120000), Validators.pattern(/^-?(0|[1-9]\d*)?$/)]],
      // imageURL: ['', Validators.required],
      contactPhone: ['', Validators.required],
      contactEmail: ['', [Validators.required, Validators.email]]
    });
  }
  // Salvar/editar evento
  saveEditions(template: ModalDirective) {
    if (this.registerForm.valid) {
      // copiar evento
      if (!this.event) {
        this.event = Object.assign(this.registerForm.value);
      } else {
        this.event = Object.assign({ id: this.event.id }, this.registerForm.value);
      }
      this.splitImage();

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

  splitImage() {
    if(this.event.imageURL){
      const fileName = this.event.imageURL.split('\\', 3);
      this.event.imageURL = fileName[2];

      this.eventService.postUpload(this.file, fileName[2]).subscribe(); // upload de imagem
    }
  }

  onFileChange(event: any) {
    if (event.target.files && event.target.files.length) {
      this.file = event.target.files;
      console.log(event);
    }
  }

  /**
   * tratamento de erro
   * @param errorDetails
   */
  private handleErrorResponse(errorDetails: ErrorDetails): void {
    let errorMessage = '';

    // Adiciona as mensagens do array à mensagem de erro
    if (errorDetails.messages && errorDetails.messages.length > 0) {
      errorMessage += '<ul>';
      errorDetails.messages.forEach(message => {
        errorMessage += `<li>${message}</li>`;
      });

      errorMessage += '</ul>';
    }
    this.toastr.error(errorMessage, errorDetails.title, { enableHtml: true });
  }

}
