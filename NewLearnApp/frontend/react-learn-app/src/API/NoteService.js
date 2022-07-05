import axios from "axios";

export default class NoteService {
    static async getMyNotes(userId) {
        const responce = await axios.get('http://localhost:5243/api/note/user', {
            withCredentials: true,
            params: {
                id: userId
            }
        });

        return responce;
    }

    static async createNote(note) {
        const responce = await axios.post('http://localhost:5243/api/note', note, {
            withCredentials: true
        });

        return responce.data;
    }

    static async updateNote(note) {
        await axios.put('http://localhost:5243/api/note', note, {
            withCredentials: true
        });
    }

    static async deleteNote(id, timestamp) {
        await axios.delete('http://localhost:5243/api/note', {
            data: {
                id: id,
                timestamp: timestamp
            },
            withCredentials: true
        });
    }
}