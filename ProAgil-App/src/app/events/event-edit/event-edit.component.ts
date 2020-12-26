import { Events } from './../../models/Events';
import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { EventService } from 'src/app/services/event/event.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-event-edit',
  templateUrl: './event-edit.component.html',
  styleUrls: ['./event-edit.component.css']
})
export class EventEditComponent implements OnInit {

  title = 'Editar Evento';
  event: Events;
  imageURL: 'assets/img/upload_image.jpg';
  registerForm: FormGroup;

  constructor(
    private eventService: EventService,
    private fb: FormBuilder,
    private toastr: ToastrService,
    private router: ActivatedRoute) { }

  ngOnInit(): void {
    this.validation();
    this.getEvent();
  }

  validation() {
    this.registerForm = this.fb.group({
      theme: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      eventDate: ['', Validators.required],
      personQtd: ['', [Validators.required, Validators.max(120000), Validators.pattern(/^-?(0|[1-9]\d*)?$/)]],
      contactPhone: ['', Validators.required],
      contactEmail: ['', [Validators.required, Validators.email]],
      imageURL: [''],
      lots: this.fb.array([this.createLot()]),
      networks: this.fb.array([this.createSocialNetWork()])
    });
  }

  getEvent() {
    this.event = new Events();
    const id = +this.router.snapshot.paramMap.get('id'); // o sinal de + converte para number
    this.eventService.getEvent(id)
      .subscribe(
        (event: Events) => {
          this.event = Object.assign({}, event);
          //console.log(this.event); // depois remover
        }
      );
  }

  /** adiciona vários lotes */
  createLot(): FormGroup {
    return this.fb.group({
      name: ['', Validators.required],
      qty: ['', Validators.required],
      price: ['', Validators.required],
      startDate: [''],
      endDate: [''],
    });
  }

  /** adiciona várias redes sociais */
  createSocialNetWork(): FormGroup {
    return this.fb.group({
      name: ['', Validators.required],
      url: ['', Validators.required],
    });
  }

  /** retorna lotes */
  get lots(): FormArray {
    return <FormArray>this.registerForm.get('lots');
  }

  /** retorna redes sociais */
  get networks(): FormArray {
    return <FormArray>this.registerForm.get('networks');
  }

  /**Adiciona um lote */
  addLot() {
    this.lots.push(this.createLot());
  }

  /**Adiciona uma rede social */
  addSocialNetWork() {
    this.networks.push(this.createSocialNetWork());
  }

  /**Exlui um lote */
  removeLot(id: number) {
    this.lots.removeAt(id);
  }

  /**Exlui uma rede social */
  removeSocialNetWork(id: number) {
    this.networks.removeAt(id);
  }

  onFileChange(file: FileList) {
    const reader = new FileReader();
    reader.onload = (event: any) => this.imageURL = event.target.result;
    reader.readAsDataURL(file[0]);
  }
}
