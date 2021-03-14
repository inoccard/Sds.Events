import { SpeakerEvent } from './SpeakerEvent';
import { Lot } from './Lot';
import { SocialNetwork } from './SocialNetwork';

export class Events {
    constructor() {
    }
    
    id: number;
    theme: string;
    local: string;
    eventDate: Date;
    personQtd: number;
    imageURL: string;
    contactPhone: string;
    contactEmail: string;
    lots: Lot[];
    socialNetworks: SocialNetwork[];
    speakerEvents: SpeakerEvent[];
}
