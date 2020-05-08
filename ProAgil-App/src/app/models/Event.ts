import { SpeakerEvent } from './SpeakerEvent';
import { Lot } from './Lot';
import { SocialNetwork } from './SocialNetwork';

export interface Event {
    id: number;
    eventDate: Date;
    theme: string;
    personQtd: number;
    imageURL: string;
    contactPhone: string;
    contactEmail: string;
    lots: Lot[];
    socialNetWorks: SocialNetwork[];
    speakerEvents: SpeakerEvent[];
}
