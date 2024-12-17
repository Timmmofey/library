"use client";
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { Form, Button, Table, DatePicker, Select, Alert, Checkbox } from 'antd';
import { NextPage } from 'next';
import moment from 'moment';

interface ItemCopyDto1 {
    itemCopyId: string;
    itemId: string;
    title: string;
    authors: string[];
    inventoryNumber: string;
    loanable: boolean;
    loaned: boolean;
    lost: boolean;
    dateReceived: string | null;
    dateWithdrawn: string | null;
    publications: string;
}

interface ReaderResponse {
    id: string;
    fullName: string;
}

const { RangePicker } = DatePicker;
const { Option } = Select;

const ItemsByReaderPage: NextPage = () => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);
    const [items, setItems] = useState<ItemCopyDto1[]>([]);
    const [readers, setReaders] = useState<ReaderResponse[]>([]);
    const [noResults, setNoResults] = useState<boolean>(false);

    useEffect(() => {
        const fetchReaders = async () => {
            try {
                const response = await axios.get<ReaderResponse[]>('http://localhost:5251/api/Reader');
                setReaders(response.data);
            } catch (error) {
                console.error('Error fetching readers:', error);
            }
        };

        fetchReaders();
    }, []);

    const handleSearch = async (values: any) => {
        setLoading(true);
        setItems([]);
        setNoResults(false);

        try {
            const [startDate, endDate] = values.dateRange;
            const formattedStartDate = startDate.toISOString();
            const formattedEndDate = endDate.toISOString();

            const response = await axios.get<ItemCopyDto1[]>(
                `http://localhost:5251/api/Reader/itemsByReaderAndRegistrationStatus56`, {
                    params: {
                        readerId: values.readerId,
                        startDate: formattedStartDate,
                        endDate: formattedEndDate,
                        isRegistered: values.isRegistered,
                    },
                });

            setItems(response.data);
            setNoResults(response.data.length === 0);
        } catch (error) {
            console.error('Error fetching items:', error);
            setNoResults(true);
        } finally {
            setLoading(false);
        }
    };

    const columns = [
        {
            title: 'Название',
            dataIndex: 'title',
            key: 'title',
        },
        {
            title: 'Автор(ы)',
            dataIndex: 'authors',
            key: 'authors',
            render: (authors: string[]) => authors.join(', '),
        },
        {
            title: 'Инвентарный номер',
            dataIndex: 'inventoryNumber',
            key: 'inventoryNumber',
        },
        {
            title: 'Для выдачи',
            dataIndex: 'loanable',
            key: 'loanable',
            render: (loanable: boolean) => loanable ? 'Yes' : 'No',
        },
        {
            title: 'Выдана',
            dataIndex: 'loaned',
            key: 'loaned',
            render: (loaned: boolean) => loaned ? 'Yes' : 'No',
        },
        {
            title: 'Потеряна',
            dataIndex: 'lost',
            key: 'lost',
            render: (lost: boolean) => lost ? 'Yes' : 'No',
        },
        {
            title: 'Дата получения',
            dataIndex: 'dateReceived',
            key: 'dateReceived',
            render: (date: string | null) => date ? moment(date).format('MM-DD-YYYY') : 'N/A',
        },
        {
            title: 'Дата списания',
            dataIndex: 'dateWithdrawn',
            key: 'dateWithdrawn',
            render: (date: string | null) => date ? moment(date).format('MM-DD-YYYY') : 'N/A',
        },
        {
            title: 'Произведения',
            dataIndex: 'publications',
            key: 'publications',
        },
    ];

    return (
        <div>
            <h1>5. Выдать список изданий, которые в течение некоторого времени получал указанный 
                читатель из фонда библиотеки, где он зарегистрирован. 
                6. Получить перечень изданий, которыми в течение некоторого времени пользовался 
                указанный читатель из фонда библиотеки, где он не зарегистрирован.</h1>
            <Form
                form={form}
                layout="vertical"
                onFinish={handleSearch}
                initialValues={{
                    isRegistered: false,
                }}
            >
                <Form.Item name="readerId" label="Имя читателя" rules={[{ required: true, message: 'Please select a reader' }]}>
                    <Select placeholder="Select Reader">
                        {readers.map(reader => (
                            <Option key={reader.id} value={reader.id}>
                                {reader.fullName}
                            </Option>
                        ))}
                    </Select>
                </Form.Item>

                <Form.Item name="dateRange" label="Выберите промежуток" rules={[{ required: true, message: 'Please select a date range' }]}>
                    <RangePicker format="YYYY-MM-DD" />
                </Form.Item>

                <Form.Item name="isRegistered" valuePropName="checked">
                    <Checkbox>Зарегистрирован в библиотеке</Checkbox>
                </Form.Item>

                <Form.Item>
                    <Button type="primary" htmlType="submit" loading={loading}>
                        Поиск
                    </Button>
                </Form.Item>
            </Form>

            {noResults && <Alert message="No items found for the specified reader in the given date range." type="info" showIcon style={{ marginBottom: '16px' }} />}

            <Table
                dataSource={items}
                columns={columns}
                rowKey="itemCopyId"
                loading={loading}
                pagination={{ pageSize: 10 }}
                locale={{ emptyText: 'Нет записей' }}
            />
        </div>
    );
};

export default ItemsByReaderPage;
