"use client";
import React, { useState } from 'react';
import axios from 'axios';
import { Table, Button, Alert, DatePicker, Form, Radio  } from 'antd';
import dayjs from 'dayjs';

const { RangePicker } = DatePicker;

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
    publications: string | null;
}

const ItemCopiesPage: React.FC = () => {
    const [loading, setLoading] = useState(false);
    const [itemCopies, setItemCopies] = useState<ItemCopyDto1[]>([]);
    const [noResults, setNoResults] = useState(false);
    const [includeReceived, setIncludeReceived] = useState(true);
    const [includeWithdrawn, setIncludeWithdrawn] = useState(false);

    const fetchItemCopies = async (values: any) => {
        setLoading(true);
        setNoResults(false);
        setItemCopies([]);

        const [startDate, endDate] = values.dateRange;
        const startDateString = dayjs(startDate).toISOString();
        const endDateString = dayjs(endDate).toISOString();

        try {
            const response = await axios.get<ItemCopyDto1[]>('http://localhost:5251/api/Reader/itemCopies11', {
                params: {
                    startDate: startDateString,
                    endDate: endDateString,
                    includeReceived,
                    includeWithdrawn,
                },
            });
            setItemCopies(response.data);
            setNoResults(response.data.length === 0);
        } catch (error) {
            console.error('Error fetching item copies:', error);
            setNoResults(true); 
        } finally {
            setLoading(false);
        }
    };

    const onRadioChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const selectedOption = e.target.value;
        setIncludeReceived(selectedOption === 'received');
        setIncludeWithdrawn(selectedOption === 'withdrawn');
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
            render: (loanable: boolean) => (loanable ? 'Yes' : 'No'),
        },
        {
            title: 'Выдано',
            dataIndex: 'loaned',
            key: 'loaned',
            render: (loaned: boolean) => (loaned ? 'Yes' : 'No'),
        },
        {
            title: 'Потеряно',
            dataIndex: 'lost',
            key: 'lost',
            render: (lost: boolean) => (lost ? 'Yes' : 'No'),
        },
        {
            title: 'Дата получения',
            dataIndex: 'dateReceived',
            key: 'dateReceived',
            render: (date: string | null) => date ? new Date(date).toLocaleDateString() : 'N/A',
        },
        {
            title: 'Дата списания',
            dataIndex: 'dateWithdrawn',
            key: 'dateWithdrawn',
            render: (date: string | null) => date ? new Date(date).toLocaleDateString() : 'N/A',
        },
        {
            title: 'Произведения',
            dataIndex: 'publications',
            key: 'publications',
            render: (publications: string | null) => publications || 'N/A',
        }
    ];

    return (
        <div>
            <h1>Получить перечень указанной литературы, которая поступила (была списана) в 
            течение некоторого периода. </h1>
            <Form onFinish={fetchItemCopies} layout="inline">
                <Form.Item
                    name="dateRange"
                    label="Промежуток времени"
                    rules={[{ required: true, message: 'Please select a date range' }]}
                >
                    <RangePicker format="YYYY-MM-DD" />
                </Form.Item>
                
                <Form.Item label="Include">
                    <Radio.Group onChange={onRadioChange} defaultValue="received">
                        <Radio value="received">Получены</Radio>
                        <Radio value="withdrawn">Списаны</Radio>
                    </Radio.Group>
                </Form.Item>

                <Form.Item>
                    <Button type="primary" htmlType="submit" loading={loading}>
                        Поиск
                    </Button>
                </Form.Item>
            </Form>

            {noResults && (
                <Alert
                    message="No item copies found for the specified date range."
                    type="info"
                    showIcon
                    style={{ marginTop: '16px', marginBottom: '16px' }}
                />
            )}

            <Table
                dataSource={itemCopies}
                columns={columns}
                rowKey="itemCopyId"
                loading={loading}
                pagination={{ pageSize: 10 }}
                locale={{ emptyText: 'Нет записей' }}
            />
        </div>
    );
};

export default ItemCopiesPage;
