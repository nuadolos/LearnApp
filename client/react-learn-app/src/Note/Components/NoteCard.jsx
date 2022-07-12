import React from "react";
import { useNavigate } from "react-router-dom";
import ModalLink from "../../Components/UI/Modal/ModalLink";

const NoteCard = ({ note, setIsUpdateNote, remove }) => {
    const setDataNote = (n) => {
        setIsUpdateNote({
            note: {
                id: n.id,
                title: n.title,
                description: n.description,
                link: n.link,
                createDate: n.createDate,
                sourceLoreId: n.sourceLoreId,
                isVisible: n.isVisible,
                timestamp: n.timestamp,
                userId: n.userId
            }, isUpdate: true
        });
    }

    return (
        <div className="card border-info mb-3" style={{ maxWidth: '18rem', padding: '0', margin: '5px' }}>
            <div className="card-header">
                <div style={{ display: 'flex', justifyContent: 'space-between' }}>
                    <span>{note.isVisible ? "Публичный" : "Приватный"}</span>
                    <span>{new Date(note.createDate).toLocaleDateString()}</span>

                    <ModalLink onClick={() => setDataNote(note)} />
                    <svg onClick={() => remove(note)} style={{ float: 'right', width: '20px', cursor: 'pointer' }} xmlns="http://www.w3.org/2000/svg" viewBox="0 0 36 36">
                        <path fill="#DD2E44" d="M21.533 18.002L33.768 5.768c.976-.976.976-2.559 0-3.535-.977-.977-2.559-.977-3.535 0L17.998 14.467 5.764 2.233c-.976-.977-2.56-.977-3.535 0-.977.976-.977 2.559 0 3.535l12.234 12.234L2.201 30.265c-.977.977-.977 2.559 0 3.535.488.488 1.128.732 1.768.732s1.28-.244 1.768-.732l12.262-12.263 12.234 12.234c.488.488 1.128.732 1.768.732.64 0 1.279-.244 1.768-.732.976-.977.976-2.559 0-3.535L21.533 18.002z" />
                    </svg>
                </div>
            </div>
            <div className="card-body">
                <h3 className="card-title">{note.title}</h3>
                <h6>Описание:</h6>
                <p className="card-text">{note.description}</p>
                <h6>Ссылка на ресурс:</h6>
                <a href={note.link} target='_blank'>{note.link}</a>
            </div>
        </div>
    );
}

export default NoteCard;