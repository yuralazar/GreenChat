export class MessageContent{
    id : number;
    date: Date;
    text: string;

    constructor(id: number, date: any, text: string) {
        this.id = id;
        this.date = typeof date === "string" ? new Date(date) : date;
        this.text = text;
    }

    static create(message: any) {
       return new MessageContent(message.id , message.date, message.text);
    }
}
