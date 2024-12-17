"use client";
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Select, Button, Table, Spin } from 'antd';

const { Option } = Select;

interface Author {
    id: string;
    name: string;
}

interface ItemCopy {
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

const SearchItemsByAuthorsPage: React.FC = () => {
    const [authors, setAuthors] = useState<Author[]>([]);
    const [selectedAuthorIds, setSelectedAuthorIds] = useState<string[]>([]);
    const [itemCopies, setItemCopies] = useState<ItemCopy[]>([]);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const fetchAuthors = async () => {
            try {
                const response = await axios.get<Author[]>('http://localhost:5251/Authors'); 
                setAuthors(response.data);
            } catch (error) {
                console.error('Error fetching authors:', error);
            }
        };

        fetchAuthors();
    }, []);

    const handleSearch = async () => {
        setLoading(true);
        setItemCopies([]); 

        try {
            const response = await axios.post<ItemCopy[]>('http://localhost:5251/api/Reader/searchItems15', selectedAuthorIds);
            if (response.data.length === 0) {
            } else {
                setItemCopies(response.data); 
            }
        } catch (error) {
            console.error('Error fetching items:', error);
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
            render: (text: string | null) => (text ? new Date(text).toLocaleDateString() : '-'), 
        },
        {
            title: 'Дата списания',
            dataIndex: 'dateWithdrawn',
            key: 'dateWithdrawn',
            render: (text: string | null) => (text ? new Date(text).toLocaleDateString() : '-'), 
        },
        {
            title: 'Произведения',
            dataIndex: 'publications',
            key: 'publications',
        },
    ];

    return (
        <div>
            <h1>Выдать список инвентарных номеров и названий из библиотечного фонда, в которых 
            содержатся произведения указанного автора.</h1>
            <Select
                mode="multiple"
                placeholder="Выберите авторов"
                value={selectedAuthorIds}
                onChange={setSelectedAuthorIds}
                style={{ width: 300, marginBottom: 16 }}
            >
                {authors.map(author => (
                    <Option key={author.id} value={author.id}>
                        {author.name}
                    </Option>
                ))}
            </Select>

            <Button type="primary" onClick={handleSearch} disabled={loading || selectedAuthorIds.length === 0}>
                Поиск
            </Button>

            {loading && <Spin style={{ marginTop: '16px' }} />}

            <Table
                dataSource={itemCopies}
                columns={columns}
                rowKey="itemCopyId"
                pagination={{ pageSize: 10 }}
                locale={{ emptyText: loading ? 'Loading...' : 'Нет данных' }}
                style={{ marginTop: '16px' }}
            />
        </div>
    );
};

export default SearchItemsByAuthorsPage;
