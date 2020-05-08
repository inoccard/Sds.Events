import { SocialNetwork } from './SocialNetwork';
import { SpeakerEvent } from './SpeakerEvent';
export interface Speaker {
    id: number;
    name: string;
    miniCurriculum: string;
    imageUrl: string;
    contactCell: string;
    contactEmail: string;
    socialNetworks: SocialNetwork[];
    speakerEvents: SpeakerEvent[];
}
