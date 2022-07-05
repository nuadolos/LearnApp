export default class SourceLoreService {
    static async getSourceAll() {
        const responce = await fetch('http://localhost:5243/api/source', {
            method: 'GET',
            headers: { 'Content-Types': 'application/json' },
            credentials: 'include'
        });

        return await responce.json();
    }
}