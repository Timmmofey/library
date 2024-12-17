"use client"
"use client";

import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Form, Button, Table, Select, Alert } from 'antd';
import { NextPage } from 'next';

interface ReaderDto1 {
    id: string;
    email: string;
    fullName: string;
    libraryName: string | null;
    readerCategoryId: string | null; 
    subscriptionEndDate: string | null; 
    educationalInstitution: string | null;
    faculty: string | null;
    course: string | null;
    groupNumber: string | null;
    organization: string | null;
    researchTopic: string | null;
}

interface ItemResponse {
    id: string;
    title: string;
}

const { Option } = Select;

const ReadersPage: NextPage = () => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);
    const [readers, setReaders] = useState<ReaderDto1[]>([]);
    const [items, setItems] = useState<ItemResponse[]>([]);
    const [selectedItemId, setSelectedItemId] = useState<string | null>(null);
    const [noResults, setNoResults] = useState<boolean>(false);

    useEffect(() => {
        const fetchItems = async () => {
            try {
                const response = await axios.get<ItemResponse[]>('http://localhost:5251/Items');
                setItems(response.data);
            } catch (error) {
                console.error('Error fetching items:', error);
            }
        };

        fetchItems();
    }, []);

    const handleSearch = async () => {
        if (!selectedItemId) return;

        setLoading(true);
        setReaders([]); 
        setNoResults(false);

        try {
            const response = await axios.get<ReaderDto1[]>(`http://localhost:5251/api/Reader/withItem3/${selectedItemId}`);
            setReaders(response.data);
            setNoResults(response.data.length === 0);
        } catch (error) {
            console.error('Error fetching readers:', error);
            setNoResults(true);
        } finally {
            setLoading(false);
        }
    };

    const formatDate = (dateString: string | null) => {
        if (!dateString) return null;
        const date = new Date(dateString);
        const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: '2-digit', day: '2-digit' };
        return date.toLocaleDateString('en-GB', options);
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
            title: 'Библиотека',
            dataIndex: 'libraryName',
            key: 'libraryName',
        },
        {
            title: 'Категория читателя',
            dataIndex: 'readerCategoryId', 
            key: 'readerCategoryId',
            render: (id: string) => {
                switch (id) {
                    case 'ceb525a5-8d68-4bd6-a962-e6e5f1a0ff3c':
                        return 'Студент';
                    case 'ddb7a5f0-0e41-4509-b904-6abc77611f81':
                        return 'Научный сотрудник';
                    default:
                        return 'Неизвестно';
                }
            },
        },
        {
            title: 'Дата окончания подписки',
            dataIndex: 'subscriptionEndDate',
            key: 'subscriptionEndDate',
            render: (date: string | null) => formatDate(date), 
        },
        {
            title: 'Учебное заведение',
            dataIndex: 'educationalInstitution',
            key: 'educationalInstitution',
        },
        {
            title: 'Факультет',
            dataIndex: 'faculty',
            key: 'faculty',
        },
        {
            title: 'Курс',
            dataIndex: 'course',
            key: 'course',
        },
        {
            title: 'Группа',
            dataIndex: 'groupNumber',
            key: 'groupNumber',
        },
        {
            title: 'Организация',
            dataIndex: 'organization',
            key: 'organization',
        },
        {
            title: 'Тема исследования',
            dataIndex: 'researchTopic',
            key: 'researchTopic',
        },
    ];

    return (
        <div>
            <h1>Получить список читателей, на руках у которых находится указанное издание 
            (книга, журнал и т.д). </h1>
            <Form 
                form={form} 
                layout="vertical" 
                initialValues={{ itemId: '' }} 
            >
                <Form.Item name="itemId" label="Выберите предмет">
                    <Select 
                        placeholder="Выберите предмет" 
                        onChange={setSelectedItemId}
                        allowClear
                    >
                        {items.map(item => (
                            <Option key={item.id} value={item.id}>
                                {item.title}
                            </Option>
                        ))}
                    </Select>
                </Form.Item>

                <Form.Item>
                    <Button type="primary" onClick={handleSearch} loading={loading}>
                        Поиск
                    </Button>
                </Form.Item>
            </Form>

            {noResults && <Alert message="Нет записей, соответствующих вашему запросу." type="info" showIcon style={{ marginBottom: '16px' }} />}

            {!loading && readers.length === 0 && !noResults ? (
                <Alert message="Нет данных" type="info" showIcon style={{ marginBottom: '16px' }} />
            ) : (
                <Table
                    dataSource={readers}
                    columns={columns}
                    rowKey="id"
                    loading={loading}
                    pagination={{ pageSize: 10 }}
                    locale={{ emptyText: 'Нет данных' }} 
                />
            )}
        </div>
    );
};

export default ReadersPage;
