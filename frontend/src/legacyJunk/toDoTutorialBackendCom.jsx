import { useState, useEffect } from 'react';

export default function TodoApp() {
    const [todos, setTodos] = useState([]);
    const [inputValue, setInputValue] = useState("");

    // Lädt die vorhandenen Todos beim Start
    useEffect(() => {
        fetch('/backend/todoitems')
            .then(res => res.json())
            .then(data => setTodos(data))
            .catch(err => console.error("Ladefehler:", err));
    }, []);

    const handleAddTodo = async (e) => {
        e.preventDefault();
        if (!inputValue.trim()) return;
        const newTodo = {
            name: inputValue,
            isComplete: false
        };

        try {
            const response = await fetch('/backend/todoitems', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(newTodo)
            });

            if (response.ok) {
                const createdTodo = await response.json();
                setTodos([...todos, createdTodo]); // Liste im UI aktualisieren
                setInputValue(""); // Feld leeren
            }
        } catch (error) {
            console.error("Fehler beim Erstellen:", error);
        }
    };

    return (
        <div style={{ padding: '20px', fontFamily: 'sans-serif' }}>
            <h1>Meine Todos</h1>
            <form onSubmit={handleAddTodo}>
                <input
                    value={inputValue}
                    onChange={(e) => setInputValue(e.target.value)}
                    placeholder="Neue Aufgabe..."
                />
                <button type="submit">Hinzufügen</button>
            </form>

            <ul>
                {todos.map(todo => (
                    <li key={todo.id}>
                        {todo.name} {todo.isComplete ? "(Erledigt)" : ""}
                    </li>
                ))}
            </ul>
        </div>
    );
}