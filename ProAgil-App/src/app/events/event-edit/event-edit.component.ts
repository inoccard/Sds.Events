import { SocialNetwork } from './../../models/SocialNetwork';
import { Lot } from './../../models/Lot';
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
  event: Events = new Events();
  imageURL = 'assets/img/upload_image.jpg';
  registerForm: FormGroup;
  fileNameToUpdate: string;
  currentDate: any;

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
      lots: this.fb.array([]),
      networks: this.fb.array([])
    });
  }

  /** carrega evento, seu lotes e redes sociais */
  getEvent() {
    const id = +this.router.snapshot.paramMap.get('id'); // o sinal de + converte para number
    this.eventService.getEvent(id)
      .subscribe(
        (event: Events) => {
          this.event = Object.assign({}, event);
          this.fileNameToUpdate = event.imageURL.toString();
          
          this.imageURL = `http://localhost:5000/resources/images/${this.event.imageURL}?_ts=${this.currentDate}`;
          this.event.imageURL = '';
          this.registerForm.patchValue(this.event);

          this.event.lots.forEach(lot => {
            this.lots.push(this.createLot(lot));
          });

          this.event.socialNetWorks.forEach(snw => {
            this.networks.push(this.createSocialNetWork(snw));
          });
        }
      );
  }

  /** adiciona lote */
  createLot(lot: any): FormGroup {
    return this.fb.group({
      id: [lot.id],
      name: [lot.name, Validators.required],
      qty: [lot.qty, Validators.required],
      price: [lot.price, Validators.required],
      startDate: [lot.startDate],
      endDate: [lot.endDate],
    });
  }

  /** adiciona rede social */
  createSocialNetWork(socialNetwork: any): FormGroup {
    return this.fb.group({
      id: [socialNetwork.id],
      name: [socialNetwork.name, Validators.required],
      url: [socialNetwork.url, Validators.required],
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
    this.lots.push(this.createLot({ id: 0 }));
  }

  /**Adiciona uma rede social */
  addSocialNetWork() {
    this.networks.push(this.createSocialNetWork({ id: 0 }));
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
