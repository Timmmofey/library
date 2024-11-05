"use client";
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Select, Table, Form, Button, DatePicker } from 'antd';
import moment from 'moment';

interface LibrarianResponse {
    id: string;
    name: string;
    readingRoomId?: string;
}

interface Reader {
    id: string;
    email: string;
    fullName: string;
    subscriptionEndDate: Date | null;
}

const ReadersServicedByLibrarian: React.FC = () => {
    const [librarians, setLibrarians] = useState<LibrarianResponse[]>([]);
    const [selectedLibrarianId, setSelectedLibrarianId] = useState<string>('');
    const [dateRange, setDateRange] = useState<[moment.Moment | null, moment.Moment | null]>([null, null]);
    const [readers, setReaders] = useState<Reader[]>([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const fetchLibrarians = async () => {
            try {
                const response = await axios.get('http://localhost:5251/Librarians'); 
                setLibrarians(response.data);
            } catch (error) {
                console.error('Error fetching librarians', error);
            }
        };

        fetchLibrarians();
    }, []);

    const handleSubmit = async () => {
        const [startDate, endDate] = dateRange;

        if (!selectedLibrarianId) {
            return;
        }
        
        if (!startDate || !endDate) {
            return;
        }

        setLoading(true);
        setReaders([]); 

        try {
            const response = await axios.get(
                `http://localhost:5251/api/Reader/readersServicedByLibrarian8`, 
                { params: { librarianId: selectedLibrarianId, startDate: startDate.toISOString(), endDate: endDate.toISOString() } }
            );
            setReaders(response.data);
        } catch (error) {
            console.error('Error fetching readers serviced by the librarian', error);
        } finally {
            setLoading(false);
        }
    };

    const columns = [
        {
            title: 'Имя',
            dataIndex: 'fullName',
            key: 'fullName',
        },
        {
            title: 'Email',
            dataIndex: 'email',
            key: 'email',
        },
        {
            title: 'Дата конца подписки',
            dataIndex: 'subscriptionEndDate',
            key: 'subscriptionEndDate',
            render: (date: Date | null) => date ? new Date(date).toLocaleDateString() : 'N/A',
        },
    ];

    return (
        <div>
            <h1>Выдать список читателей, которые в течение обозначенного периода были 
            обслужены указанным библиотекарем. </h1>
            <Form layout="inline" onFinish={handleSubmit}>
                <Form.Item label="Выберите библиотекаря:">
                    <Select
                        style={{ width: 200 }}
                        value={selectedLibrarianId}
                        onChange={setSelectedLibrarianId}
                        placeholder="Выберите библиотекаря"
                    >
                        {librarians.map(librarian => (
                            <Select.Option key={librarian.id} value={librarian.id}>
                                {librarian.name}
                            </Select.Option>
                        ))}
                    </Select>
                </Form.Item>

                <Form.Item label="Промежуток времени:">
                    <DatePicker.RangePicker
                        value={dateRange}
                        onChange={setDateRange}
                    />
                </Form.Item>

                <Form.Item>
                    <Button type="primary" htmlType="submit" loading={loading}>Поиск</Button>
                </Form.Item>
            </Form>

            <Table
                dataSource={readers}
                columns={columns}
                rowKey="id"
                loading={loading}
                pagination={{ pageSize: 10 }}
                locale={{ emptyText: 'Нет данных' }}
            />
        </div>
    );
};

export default ReadersServicedByLibrarian;
