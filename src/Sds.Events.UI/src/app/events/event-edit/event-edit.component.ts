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
  file: FileList;

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
      id: [],
      theme: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      eventDate: ['', Validators.required],
      personQtd: ['', [Validators.required, Validators.max(120000), Validators.pattern(/^-?(0|[1-9]\d*)?$/)]],
      contactPhone: ['', Validators.required],
      contactEmail: ['', [Validators.required, Validators.email]],
      imageURL: [''],
      lots: this.fb.array([]),
      socialNetworks : this.fb.array([])
    });
  }

  /** carrega evento, seu lotes e redes sociais */
  getEvent() {
    const id = +this.router.snapshot.paramMap.get('id'); // o sinal de + converte para number
    this.eventService.getEvent(id)
      .subscribe(
        (event: Events) => {
          this.event = Object.assign({}, event);

          if(this.event.imageURL){
            this.fileNameToUpdate = this.event.imageURL.toString();
            this.imageURL = `http://localhost:5000/resources/images/${this.event.imageURL}?_ts=${this.currentDate}`;
          }

          this.event.imageURL = '';
          this.registerForm.patchValue(this.event);

          this.event.lots.forEach(lot => {
            this.lots.push(this.createLot(lot));
          });

          this.event.socialNetworks.forEach(snw => {
            this.socialNetworks.push(this.createSocialNetWork(snw));
          });
        }
      );
  }

  /** adiciona lote */
  createLot(lot: Lot): FormGroup {
    return this.fb.group({
      id: [lot.id ?? 0],
      name: [lot.name, Validators.required],
      qty: [lot.qty, Validators.required],
      price: [lot.price, Validators.required],
      initDate: [lot.initDate],
      endDate: [lot.endDate],
    });
  }

  /** adiciona rede social */
  createSocialNetWork(socialNetwork: SocialNetwork): FormGroup {
    return this.fb.group({
      id: [socialNetwork.id ?? 0],
      name: [socialNetwork.name, Validators.required],
      url: [socialNetwork.url, Validators.required],
    });
  }

  /** retorna lotes */
  get lots(): FormArray {
    let lots = <FormArray>this.registerForm.get('lots');
    return lots;
  }

  /** retorna redes sociais */
  get socialNetworks(): FormArray {
    let socialNetworks = <FormArray>this.registerForm.get('socialNetworks');
    return socialNetworks;
  }

  /**Adiciona um lote */
  addLot() {
    this.lots.push(this.createLot(new Lot()));
  }

  /**Adiciona uma rede social */
  addSocialNetWork() {
    this.socialNetworks.push(this.createSocialNetWork(new SocialNetwork()));
  }

  /**Exlui um lote */
  removeLot(id: number) {
    this.lots.removeAt(id);
  }

  /**Exlui uma rede social */
  removeSocialNetWork(id: number) {
    this.socialNetworks.removeAt(id);
  }

  onFileChange(file: FileList) {
    const reader = new FileReader();
    reader.onload = (event: any) => this.imageURL = event.target.result;
    this.file = file;
    reader.readAsDataURL(file[0]);
  }

  Save() {
    if (this.registerForm.valid) {
      // copiar evento
      if (!this.event) {
        this.event = Object.assign(this.registerForm.value);
      } else {
        this.event = Object.assign({ id: this.event.id }, this.registerForm.value);
      }
      this.event.imageURL = this.fileNameToUpdate;
      
      this.splitImage();

      this.eventService.saveEvent(this.event).subscribe(
        (newEvent: Events) => {
          this.toastr.success(`Evento ${this.event.theme} salvo com sucesso`, 'Salvar');
        }, error => {
          console.log(error);
          this.toastr.error(`Não foi possível salvar evento ${this.event.theme}: ${error}`, 'Salvar');
        }
      );
    }
  }

  splitImage() {
    if(this.registerForm.get('imageURL').value !== ''){

      this.eventService.postUpload(this.file, this.registerForm.get('imageURL').value).subscribe(
        () => {
          this.currentDate = new Date().getMilliseconds().toString();
          this.imageURL =  `http://localhost:5000/resources/images/${this.event.imageURL}_ts=${this.currentDate}`;
        }
      ); // upload de imagem
    }
  }
}
