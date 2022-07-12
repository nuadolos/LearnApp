import { toHaveErrorMessage } from "@testing-library/jest-dom/dist/matchers";
import { keyboard } from "@testing-library/user-event/dist/keyboard";
import React from "react";
import { useContext } from "react";
import { useEffect } from "react";
import { useState } from "react";
import NoteService from "../../API/NoteService";
import SourceLoreService from "../../API/SourceLoreService";
import Loader from "../../Components/UI/Loader/Loader";
import Select from "../../Components/UI/Select/Select";
import { AuthContext } from "../../Context/AuthContext";
import { useFetching } from "../../Hooks/useFetching";

const NoteForm = ({ addEditArray, isUpdateNote }) => {
    const { userId } = useContext(AuthContext);

    const [note, setNote] = useState({
        id: 0,
        title: '',
        description: '',
        link: '',
        createDate: new Date().toJSON(),
        sourceLoreId: '',
        isVisible: false,
        userId: userId,
        timestamp: ''
    });
    const [sources, setSources] = useState([]);
    const [error, setError] = useState({ title: '', link: '', sourceLoreId: '' });

    const [getSources, isSourceLoading, sourceError] = useFetching(async () => {
        const content = await SourceLoreService.getSourceAll();
        setSources(content);
    })

    useEffect(() => {
        getSources();
    }, []);

    useEffect(() => {
        const n = isUpdateNote.note;
        setNote({
            id: n.id,
            title: n.title,
            description: n.description ? n.description : '',
            link: n.link,
            createDate: n.createDate,
            sourceLoreId: n.sourceLoreId,
            isVisible: n.isVisible,
            timestamp: n.timestamp,
            userId: n.userId
        });
    }, [isUpdateNote]);

    const validTitle = (event) => {
        setNote({ ...note, title: event.target.value });

        if (event.target.value === '') {
            setError({ ...error, title: 'Укажите название заметки' });
            document.getElementById('title').className = 'form-control is-invalid';
        }
        else {
            setError({ ...error, title: '' });
            document.getElementById('title').className = 'form-control is-valid';
        }
    }

    const validLink = (event) => {
        setNote({ ...note, link: event.target.value });

        if (event.target.value === '') {
            setError({ ...error, link: 'Укажите название заметки' });
            document.getElementById('link').className = 'form-control is-invalid';
        }
        else {
            setError({ ...error, link: '' });
            document.getElementById('link').className = 'form-control is-valid';
        }
    }

    const validLoreId = (event) => {
        setNote({ ...note, sourceLoreId: event.target.value });

        if (event.target.value === '') {
            setError({ ...error, sourceLoreId: 'Укажите название заметки' });
            document.getElementById('sourceLoreId').className = 'form-control is-invalid';
        }
        else {
            setError({ ...error, sourceLoreId: '' });
            document.getElementById('sourceLoreId').className = 'form-control is-valid';
        }
    }

    const addEditNote = async (event) => {
        event.preventDefault();

        if (error.title || error.link || error.sourceLoreId) {
            return false;
        }

        if (isUpdateNote.isUpdate) {
            await NoteService.updateNote(note);
            addEditArray(note);
        }
        else {
            const data = await NoteService.createNote(note);
            addEditArray(data);
        }

        document.getElementById('title').className = 'form-control';
        document.getElementById('link').className = 'form-control';
        document.getElementById('sourceLoreId').className = 'form-control';
    };

    return (
        <form onSubmit={addEditNote}>
            {isSourceLoading &&
                <Loader />
            }

            {sourceError &&
                <div class="card text-white bg-danger mb-3" style="max-width: 18rem;">
                    <div class="card-body">
                        <h5 class="card-title">Ошибка</h5>
                        <p class="card-text">{sourceError}.</p>
                    </div>
                </div>
            }

            <input type='hidden' value={note.id} />
            <input type='hidden' value={note.timestamp} />
            <input type='hidden' value={note.createDate} />
            <input type='hidden' value={note.userId} />
            <div className="mb-3">
                <label htmlFor="title" className="form-label">Наименование</label>
                <input value={note.title} onChange={event => validTitle(event)}
                    type="text" className="form-control" id="title" placeholder="Разработка веб-сервиса..." />
            </div>
            <div className="mb-3">
                <label htmlFor="description" className="form-label">Описание</label>
                <textarea value={note.description} onChange={event => setNote({ ...note, description: event.target.value })}
                    className="form-control" id="description" rows="3"></textarea>
            </div>
            <div className="mb-3 has-validation">
                <label htmlFor="link" className="form-label">Ссылка на ресурс</label>
                <input value={note.link} onChange={event => validLink(event)}
                    type="text" className="form-control" id="link" placeholder="https://example.com" />
            </div>
            <div className="mb-3">
                <label className="form-label">Источник</label>
                <Select value={note.sourceLoreId} onChange={event => validLoreId(event)}
                    options={sources} defaultValue="Источник..." id="sourceLoreId" />
            </div>
            <div className="form-check">
                <input value={note.isVisible} onChange={event => setNote({ ...note, isVisible: Boolean(event.target.value) })}
                    className="form-check-input" type="checkbox" id="isVisible" />
                <label className="form-check-label" htmlFor="isVisible">Открытый доступ?</label>
            </div>

            <div className="modal-footer">
                <button type="button" className="btn btn-secondary" data-bs-dismiss="modal">Закрыть</button>
                <button type="submit" className="btn btn-primary" data-bs-dismiss="modal" disabled={note.title && note.link && note.sourceLoreId ? null : 'disabled'}>Сохранить</button>
            </div>
        </form>
    );
};

export default NoteForm;