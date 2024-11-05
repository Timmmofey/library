"use client"
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Form, Input, Button, Table, Select, Alert } from 'antd';
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

interface LibraryResponseModel {
    id: string;
    name: string;
}

const { Option } = Select;

const ReadersPage: NextPage = () => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);
    const [readers, setReaders] = useState<ReaderDto1[]>([]);
    const [libraries, setLibraries] = useState<LibraryResponseModel[]>([]);
    const [category, setCategory] = useState<string | undefined>(undefined);
    const [noResults, setNoResults] = useState<boolean>(false);

    useEffect(() => {
        const fetchLibraries = async () => {
            try {
                const response = await axios.get<LibraryResponseModel[]>('http://localhost:5251/Library'); 
                setLibraries(response.data);
            } catch (error) {
                console.error('Ошибка при загрузке библиотек', error);
            }
        };

        fetchLibraries();
    }, []);

    const handleSearch = async (values: any) => {
        setLoading(true);
        setReaders([]);
        setNoResults(false); 
        try {
            const response = await axios.get<ReaderDto1[]>('http://localhost:5251/api/Reader/search1', { 
                params: values,
            });
            setReaders(response.data);
            setNoResults(response.data.length === 0); 
        } catch (error) {
            console.error('Ошибка при загрузке данных', error);
            setNoResults(true); 
        } finally {
            setLoading(false);
        }
    };

    const handleCategoryChange = (value: string) => {
        setCategory(value);
    };

    const formatDate = (dateString: string | null) => {
        if (!dateString) return null;
        const date = new Date(dateString);
        const options: Intl.DateTimeFormatOptions = { year: 'numeric', month: '2-digit', day: '2-digit' };
        return date.toLocaleDateString('en-US', options).split('/').reverse().join('-'); 
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
            <h1>Получить список читателей с заданными характеристиками: студентов указанного 
            учебного заведения, факультета, научных работников по определенной тематике и 
            т.д. </h1>
            <Form 
                form={form} 
                layout="vertical" 
                onFinish={handleSearch} 
                initialValues={{ 
                    readerCategoryId: '', 
                    libraryId: '', 
                }}
            >
                <Form.Item name="fullName" label="Имя">
                    <Input placeholder="Введите имя" />
                </Form.Item>

                <Form.Item name="email" label="Email">
                    <Input placeholder="Введите email" />
                </Form.Item>

                <Form.Item name="readerCategoryId" label="Категория читателя">
                    <Select 
                        onChange={handleCategoryChange} 
                        placeholder="Выберите категорию" 
                        allowClear
                    >
                        <Option value="">Не выбрано</Option>
                        <Option value="ceb525a5-8d68-4bd6-a962-e6e5f1a0ff3c">Студент</Option>
                        <Option value="ddb7a5f0-0e41-4509-b904-6abc77611f81">Научный сотрудник</Option>
                    </Select>
                </Form.Item>

                {category === 'ceb525a5-8d68-4bd6-a962-e6e5f1a0ff3c' && (
                    <>
                        <Form.Item name="educationalInstitution" label="Учебное заведение">
                            <Input placeholder="Введите учебное заведение" />
                        </Form.Item>
                        <Form.Item name="faculty" label="Факультет">
                            <Input placeholder="Введите факультет" />
                        </Form.Item>
                        <Form.Item name="course" label="Курс">
                            <Input placeholder="Введите курс" />
                        </Form.Item>
                        <Form.Item name="groupNumber" label="Номер группы">
                            <Input placeholder="Введите номер группы" />
                        </Form.Item>
                    </>
                )}

                {category === 'ddb7a5f0-0e41-4509-b904-6abc77611f81' && (
                    <>
                        <Form.Item name="organization" label="Организация">
                            <Input placeholder="Введите организацию" />
                        </Form.Item>
                        <Form.Item name="researchTopic" label="Тема исследования">
                            <Input placeholder="Введите тему исследования" />
                        </Form.Item>
                    </>
                )}

                <Form.Item name="libraryId" label="Библиотека">
                    <Select placeholder="Выберите библиотеку" allowClear>
                        <Option value="">Не выбрано</Option>
                        {libraries.map(library => (
                            <Option key={library.id} value={library.id}>
                                {library.name}
                            </Option>
                        ))}
                    </Select>
                </Form.Item>

                <Form.Item>
                    <Button type="primary" htmlType="submit" loading={loading}>
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
