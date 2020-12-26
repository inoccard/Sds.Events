import { SpeakerEvent } from './SpeakerEvent';
import { Lot } from './Lot';
import { SocialNetwork } from './SocialNetwork';

export class Events {
    id: number;
    theme: string;
    local: string;
    eventDate: Date;
    personQtd: number;
    imageURL: string;
    contactPhone: string;
    contactEmail: string;
    lots: Lot[];
    socialNetWorks: SocialNetwork[];
    speakerEvents: SpeakerEvent[];
}
