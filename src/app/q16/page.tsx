"use client";
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Table, Alert, Spin } from 'antd';

interface PopularPublication {
    publication: string;
    count: number;
}

const PopularPublicationsPage: React.FC = () => {
    const [popularPublications, setPopularPublications] = useState<PopularPublication[]>([]);
    const [loading, setLoading] = useState(true);
    const [noResults, setNoResults] = useState(false);

    useEffect(() => {
        const fetchPopularPublications = async () => {
            try {
                const response = await axios.get<PopularPublication[]>('http://localhost:5251/api/Reader/popularPublications16');
                if (response.data.length === 0) {
                    setNoResults(true);
                } else {
                    setPopularPublications(response.data);
                }
            } catch (error) {
                console.error('Error fetching popular publications:', error);
                setNoResults(true); 
            } finally {
                setLoading(false);
            }
        };

        fetchPopularPublications();
    }, []);

    const columns = [
        {
            title: 'Произведения',
            dataIndex: 'publication',
            key: 'publication',
        },
        {
            title: 'Число выдач',
            dataIndex: 'count',
            key: 'count',
        },
    ];

    return (
        <div>
            <h1>Получить список самых популярных произведений.</h1>

            {loading && <Spin style={{ marginTop: '16px' }} />}

            {noResults && (
                <Alert
                    message="No popular publications found."
                    type="info"
                    showIcon
                    style={{ marginTop: '16px', marginBottom: '16px' }}
                />
            )}

            <Table
                dataSource={popularPublications}
                columns={columns}
                rowKey="publication"
                pagination={{ pageSize: 10 }}
                locale={{ emptyText: loading ? 'Loading...' : 'Нет данных' }} 
                style={{ marginTop: '16px' }}
            />
        </div>
    );
};

export default PopularPublicationsPage;
