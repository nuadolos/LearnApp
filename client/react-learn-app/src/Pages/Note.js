import React, { useContext, useEffect, useId, useState } from "react";
import { AuthContext } from "../Context/AuthContext";
import NoteList from "../Note/Components/NoteList";
import { useFetching } from '../Hooks/useFetching';
import NoteService from "../API/NoteService";
import Loader from "../Components/UI/Loader/Loader";
import Modal from "../Components/UI/Modal/Modal";
import NoteForm from "../Note/Components/NoteForm";
import ModalButton from "../Components/UI/Modal/ModalButton";

const Note = () => {
    const { userId } = useContext(AuthContext);
    const [notes, setNotes] = useState([]);
    const [isUpdateNote, setIsUpdateNote] = useState({
        note: {},
        isUpdate: false
    });

    const [getNotes, isNoteLoading, noteError] = useFetching(async () => {
        const content = await NoteService.getMyNotes(userId);
        setNotes(content.data);
    });

    const addEditArray = (newNote) => {
        if (isUpdateNote.isUpdate) {
            setNotes(notes.map((note) =>
                note.id === newNote.id
                    ? {
                        ...note, title: newNote.title,
                        description: newNote.description,
                        link: newNote.link,
                        sourceLoreId: newNote.sourceLoreId,
                        isVisible: newNote.isVisible
                    }
                    : note
            ));
        }
        else {
            setNotes([...notes, newNote]);
        }
    };

    const setDataNull = () => {
        setIsUpdateNote({note: {
            id: 0,
            title: '',
            description: '',
            link: '',
            createDate: new Date().toJSON(),
            sourceLoreId: '',
            isVisible: false,
            userId: userId,
            timestamp: ''
        }, isUpdate: false});
    }

    useEffect(() => {
        getNotes();
    }, []);

    return (
        <div className="container">
            {isNoteLoading &&
                <Loader />
            }

            <ModalButton btnTitle="Создать заметку" onClick={setDataNull} />
            <Modal modalTitle="Создание заметки">
                <NoteForm addEditArray={addEditArray} isUpdateNote={isUpdateNote} />
            </Modal>

            {noteError &&
                <div className="card text-white bg-danger mb-3" style={{ maxWidth: '18rem' }}>
                    <div className="card-body">
                        <h5 className="card-title">Ошибка</h5>
                        <p className="card-text">{noteError}.</p>
                    </div>
                </div>
            }

            <NoteList notes={notes} setNotes={setNotes} setIsUpdateNote={setIsUpdateNote} title={'Мои заметки'} />
        </div>
    );
}

export default Note;