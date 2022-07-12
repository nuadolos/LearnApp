import React from "react";
import { CSSTransition, TransitionGroup } from "react-transition-group";
import NoteService from "../../API/NoteService";
import { useFetching } from "../../Hooks/useFetching";
import NoteCard from "./NoteCard";

const NoteList = ({ notes, setNotes, setIsUpdateNote, title }) => {
    const [removeNote, isRemoveLoading, removeError] = useFetching(async (note) => {
        await NoteService.deleteNote(note.id, note.timestamp);
        setNotes([...notes].filter(n => n.id !== note.id));
    });

    return (
        <div>
            <h1>
                {title}
            </h1>

            <TransitionGroup className='row' style={{justifyContent: 'center'}}>
                {notes.map((note) =>
                    <CSSTransition
                        key={note.id}
                        timeout={1000}
                        classNames="note">
                        <NoteCard note={note} setIsUpdateNote={setIsUpdateNote} remove={removeNote} />
                    </CSSTransition>
                )}
            </TransitionGroup>
        </div>
    );
}

export default NoteList;