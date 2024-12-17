"use client";
import React, { useState } from 'react';
import axios from 'axios';
import { Table, DatePicker, Alert, Spin } from 'antd';
import dayjs from 'dayjs';

const { RangePicker } = DatePicker;

interface Reader {
    id: string;
    fullName: string;
    email: string;
    libraryId: string;
    subscriptionEndDate: string | null; 
}

const ReadersNotVisitedPage: React.FC = () => {
    const [readers, setReaders] = useState<Reader[]>([]);
    const [loading, setLoading] = useState(false);
    const [noResults, setNoResults] = useState(false);

    const handleSubmit = async (dates: [dayjs.Dayjs, dayjs.Dayjs] | null) => {
        if (!dates || dates.length !== 2) return; 
        const sinceDate = dates[0].toISOString(); 
        const toDate = dates[1].toISOString();

        setLoading(true);
        setNoResults(false); 

        try {
            const response = await axios.get<Reader[]>(`http://localhost:5251/api/Reader/notVisited13?sinceDate=${sinceDate}&toDate=${toDate}`);
            setReaders(response.data);
            setNoResults(response.data.length === 0); 
        } catch (error) {
            console.error('Error fetching readers:', error);
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
            title: 'Дата окончания подписки',
            dataIndex: 'subscriptionEndDate',
            key: 'subscriptionEndDate',
            render: (text: string | null) => (text ? new Date(text).toLocaleString() : 'N/A'),
        },
    ];

    return (
        <div>
            <h1>Получить список читателей, не посещавших библиотеку в течение указанного 
            времени.</h1>
            <RangePicker
                style={{ marginBottom: '16px' }}
                onChange={handleSubmit}
            />
            {/* <Button 
                type="primary" 
                onClick={() => handleSubmit([dayjs(), dayjs()])} // Default to today if no dates are picked
                style={{ marginLeft: '16px' }}
            >
                Search
            </Button> */}

            {loading && <Spin style={{ marginTop: '16px' }} />}

            {noResults && (
                <Alert
                    message="No readers found who have not visited since the specified date."
                    type="info"
                    showIcon
                    style={{ marginTop: '16px', marginBottom: '16px' }}
                />
            )}

            <Table
                dataSource={readers}
                columns={columns}
                rowKey="id"
                pagination={{ pageSize: 10 }}
                locale={{ emptyText: loading ? 'Loading...' : 'Нет записей' }}
                style={{ marginTop: '16px' }}
            />
        </div>
    );
};

export default ReadersNotVisitedPage;
