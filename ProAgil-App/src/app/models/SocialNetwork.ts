import { Speaker } from './Speaker';
export interface SocialNetwork {
    id: number;
    name: string;
    url: string;
    eventId?: number;
    speakerId?: number;
}
