"use client";
import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { Select, Table, Form } from 'antd';

interface LibraryResponseModel {
    id: string;
    name: string;
    address: string;
    description: string;
}

interface ShelfDto {
    id: string;
    sectionId: string;
    number: string;
}

interface ItemCopyDto1 {
    itemCopyId: string;
    itemId: string;
    title: string;
    authors: string[];
    inventoryNumber: string;
    loanable: boolean;
    loaned: boolean;
    lost: boolean;
    dateReceived: Date | null;
    dateWithdrawn: Date | null;
    publications: string;
}

const LoanedItems: React.FC = () => {
    const [libraries, setLibraries] = useState<LibraryResponseModel[]>([]);
    const [shelves, setShelves] = useState<ShelfDto[]>([]);
    const [loanedItems, setLoanedItems] = useState<ItemCopyDto1[]>([]);
    const [selectedLibraryId, setSelectedLibraryId] = useState<string>('');
    const [selectedShelfId, setSelectedShelfId] = useState<string>('');

    const getLibraries = async (): Promise<LibraryResponseModel[]> => {
        const response = await axios.get('http://localhost:5251/Library'); 
        return response.data.map((lib: any) => ({
            id: lib.id,
            name: lib.name,
            address: lib.address,
            description: lib.description,
        }));
    };

    const getShelvesByLibraryId = async (libraryId: string): Promise<ShelfDto[]> => {
        const response = await axios.get(`http://localhost:5251/api/Reader/shelf-by-id/${libraryId}`); 
        return response.data.map((shelf: any) => ({
            id: shelf.id,
            sectionId: shelf.sectionId,
            number: shelf.number,
        }));
    };

    const getLoanedItemsByShelfId = async (shelfId: string): Promise<ItemCopyDto1[]> => {
        const response = await axios.get(`http://localhost:5251/api/Reader/loanedItemsByShelf7?shelfId=${shelfId}`);
        return response.data.map((item: any) => ({
            itemCopyId: item.itemCopyId,
            itemId: item.itemId,
            title: item.title,
            authors: item.authors,
            inventoryNumber: item.inventoryNumber,
            loanable: item.loanable,
            loaned: item.loaned,
            lost: item.lost,
            dateReceived: item.dateReceived ? new Date(item.dateReceived) : null,
            dateWithdrawn: item.dateWithdrawn ? new Date(item.dateWithdrawn) : null,
            publications: item.publications,
        }));
    };

    useEffect(() => {
        const fetchLibraries = async () => {
            const librariesData = await getLibraries();
            setLibraries(librariesData);
        };

        fetchLibraries();
    }, []);

    const handleLibraryChange = async (libraryId: string) => {
        setSelectedLibraryId(libraryId);
        setShelves([]); 
        setLoanedItems([]);

        if (libraryId) {
            const shelvesData = await getShelvesByLibraryId(libraryId);
            setShelves(shelvesData);
        }
    };

    const handleShelfChange = async (shelfId: string) => {
        setSelectedShelfId(shelfId);

        if (shelfId) {
            const loanedItemsData = await getLoanedItemsByShelfId(shelfId);
            setLoanedItems(loanedItemsData);
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
            title: 'Выдана',
            dataIndex: 'loaned',
            key: 'loaned',
            render: (loaned: boolean) => (loaned ? 'Yes' : 'No'),
        },
        {
            title: 'Потеряна',
            dataIndex: 'lost',
            key: 'lost',
            render: (lost: boolean) => (lost ? 'Yes' : 'No'),
        },
        {
            title: 'Дата получения',
            dataIndex: 'dateReceived',
            key: 'dateReceived',
            render: (dateReceived: Date | null) => dateReceived?.toLocaleDateString() || 'N/A',
        },
        {
            title: 'Дата списания',
            dataIndex: 'dateWithdrawn',
            key: 'dateWithdrawn',
            render: (dateWithdrawn: Date | null) => dateWithdrawn?.toLocaleDateString() || 'N/A',
        },
        {
            title: 'Произведения',
            dataIndex: 'publications',
            key: 'publications',
        },
    ];

    return (
        <div>
            <h1>Получить список литературы, которая в настоящий момент выдана с определенной 
            полки некоторой библиотеки. </h1>
            <Form layout="inline">
                <Form.Item label="Выберите библиотеку:">
                    <Select
                        style={{ width: 200 }}
                        value={selectedLibraryId}
                        onChange={handleLibraryChange}
                        placeholder="Select a Library"
                    >
                        {libraries.map(library => (
                            <Select.Option key={library.id} value={library.id}>
                                {library.name}
                            </Select.Option>
                        ))}
                    </Select>
                </Form.Item>

                {shelves.length > 0 && (
                    <Form.Item label="Выберите полку:">
                        <Select
                            style={{ width: 200 }}
                            value={selectedShelfId}
                            onChange={handleShelfChange}
                            placeholder="Select a Shelf"
                        >
                            {shelves.map(shelf => (
                                <Select.Option key={shelf.id} value={shelf.id}>
                                    {shelf.number}
                                </Select.Option>
                            ))}
                        </Select>
                    </Form.Item>
                )}
            </Form>

            {loanedItems.length > 0 && (
                <div style={{ marginTop: 20 }}>
                    <h2>Выданные издания:</h2>
                    <Table dataSource={loanedItems} columns={columns} rowKey="itemCopyId" />
                </div>
            )}
        </div>
    );
};

export default LoanedItems;
