"use client";
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Select, Table, Spin } from 'antd';

const { Option } = Select;

interface Library {
    id: string;
    name: string;
}

interface ReadingRoom {
    id: string;
    name: string;
}

interface Librarian {
    id: string;
    name: string;
    login: string;
    libraryId: string;
    role: string;
}

const LibrariansByReadingRoomPage: React.FC = () => {
    const [libraries, setLibraries] = useState<Library[]>([]);
    const [readingRooms, setReadingRooms] = useState<ReadingRoom[]>([]);
    const [librarians, setLibrarians] = useState<Librarian[]>([]);
    const [selectedLibraryId, setSelectedLibraryId] = useState<string | null>(null);
    const [selectedReadingRoomId, setSelectedReadingRoomId] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const fetchLibraries = async () => {
            try {
                const response = await axios.get<Library[]>('http://localhost:5251/Library'); 
                setLibraries(response.data);
            } catch (error) {
                console.error('Error fetching libraries:', error);
            }
        };

        fetchLibraries();
    }, []);

    const handleLibraryChange = async (libraryId: string) => {
        setSelectedLibraryId(libraryId);
        setSelectedReadingRoomId(null); 
        setReadingRooms([]); 
        setLibrarians([]); 

        if (libraryId) {
            try {
                const response = await axios.get<ReadingRoom[]>(`http://localhost:5251/api/Reader/reading-room-by-id/${libraryId}`);
                setReadingRooms(response.data);
            } catch (error) {
                console.error('Error fetching reading rooms:', error);
            }
        }
    };

    const handleReadingRoomChange = async (readingRoomId: string) => {
        setSelectedReadingRoomId(readingRoomId);
        setLoading(true); 

        setLibrarians([]);

        if (readingRoomId) {
            try {
                const response = await axios.get<Librarian[]>(`http://localhost:5251/api/Reader/librarians12?readingRoomId=${readingRoomId}`);
                setLibrarians(response.data);
            } catch (error) {
                console.error('Error fetching librarians:', error);
            } finally {
                setLoading(false); 
            }
        } else {
            setLoading(false);
        }
    };

    const columns = [
        {
            title: 'Имя',
            dataIndex: 'name',
            key: 'name',
        },
        {
            title: 'Логин',
            dataIndex: 'login',
            key: 'login',
        }
    ];

    return (
        <div>
            <h1>Выдать список библиотекарей, работающих в указанном читальном зале некоторой 
            библиотеки. </h1>
            <Select
                placeholder="Выбрать библиотеку"
                style={{ width: 200, marginRight: 16 }}
                onChange={handleLibraryChange}
                value={selectedLibraryId}
            >
                {libraries.map(library => (
                    <Option key={library.id} value={library.id}>
                        {library.name}
                    </Option>
                ))}
            </Select>

            <Select
                placeholder="Выбрать читальный зал"
                style={{ width: 200 }}
                onChange={handleReadingRoomChange}
                value={selectedReadingRoomId}
                disabled={!selectedLibraryId}
            >
                {readingRooms.map(room => (
                    <Option key={room.id} value={room.id}>
                        {room.name}
                    </Option>
                ))}
            </Select>

            {loading && <Spin style={{ marginTop: '16px' }} />}

            <Table
                dataSource={librarians}
                columns={columns}
                rowKey="id"
                loading={loading}
                pagination={{ pageSize: 10 }}
                locale={{ emptyText: loading ? 'Loading...' : 'Нет записей' }} 
                style={{ marginTop: '16px' }}
            />
        </div>
    );
};

export default LibrariansByReadingRoomPage;
