"use client"
import React, { useState } from 'react';
import axios from 'axios';
import { Form, Input, Button, Table, Alert } from 'antd';
import { NextPage } from 'next';

interface ReaderDto {
    id: string;
    email: string;
    fullName: string;
    libraryId: string | null;
    readerCategoryId: string | null;
    subscriptionEndDate: string | null;
    educationalInstitution: string | null;
    faculty: string | null;
    course: string | null;
    groupNumber: string | null;
    organization: string | null;
    researchTopic: string | null;
}

const ReadersWithBookPage: NextPage = () => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);
    const [readers, setReaders] = useState<ReaderDto[]>([]);
    const [noResults, setNoResults] = useState<boolean>(false);

    const handleSearch = async (values: any) => {
        setLoading(true);
        setReaders([]); 
        setNoResults(false);
        try {
            const response = await axios.get<ReaderDto[]>('http://localhost:5251/api/Reader/withBook2', {
                params: { publication: values.publication },
            });
            setReaders(response.data);
            setNoResults(response.data.length === 0); 
        } catch (error) {
            console.error('Error fetching readers with the specified publication', error);
            setNoResults(true);
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
            title: 'ID Библиотеки',
            dataIndex: 'libraryId',
            key: 'libraryId',
        },
        {
            title: 'ID Категории читателя',
            dataIndex: 'readerCategoryId',
            key: 'readerCategoryId',
        },
        {
            title: 'Дата окончания подписки',
            dataIndex: 'subscriptionEndDate',
            key: 'subscriptionEndDate',
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
            title: 'Номер группы',
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
            <h1>Выдать перечень читателей, на руках у которых находится указанное произведение. </h1>
            <Form form={form} layout="vertical" onFinish={handleSearch}>
                <Form.Item name="publication" label="Название публикации" rules={[{ required: true, message: 'Введите название публикации' }]}>
                    <Input placeholder="Введите название публикации" />
                </Form.Item>
                <Form.Item>
                    <Button type="primary" htmlType="submit" loading={loading}>
                        Поиск
                    </Button>
                </Form.Item>
            </Form>

            {noResults && <Alert message="Нет читателей, имеющих указанную публикацию." type="info" showIcon style={{ marginBottom: '16px' }} />}

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

export default ReadersWithBookPage;
