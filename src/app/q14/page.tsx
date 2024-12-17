// "use client";
// import React, { useState } from 'react';
// import axios from 'axios';
// import { Input, Button, Table, Alert, Spin } from 'antd';

// interface ItemCopy {
//     itemCopyId: string;
//     itemId: string;
//     title: string;
//     authors: string[];
//     inventoryNumber: string;
//     loanable: boolean;
//     loaned: boolean;
//     lost: boolean;
//     dateReceived: string | null; 
//     dateWithdrawn: string | null;
//     publications: string;
// }

// const ItemCopiesByPublicationPage: React.FC = () => {
//     const [publication, setPublication] = useState<string>('');
//     const [itemCopies, setItemCopies] = useState<ItemCopy[]>([]);
//     const [loading, setLoading] = useState(false);
//     const [noResults, setNoResults] = useState(false);

//     const handleSearch = async () => {
//         if (!publication) return; 

//         setLoading(true);
//         setNoResults(false); 

//         try {
//             const response = await axios.get<ItemCopy[]>(`http://localhost:5251/api/Reader/itemCopies14?publication=${publication}`);
//             setItemCopies(response.data);
//             setNoResults(response.data.length === 0); 
//         } catch (error) {
//             console.error('Error fetching item copies:', error);
//             setNoResults(true); 
//         } finally {
//             setLoading(false);
//         }
//     };

//     const columns = [
//         {
//             title: 'Название',
//             dataIndex: 'title',
//             key: 'title',
//         },
//         {
//             title: 'Автор(ы)',
//             dataIndex: 'authors',
//             key: 'authors',
//             render: (authors: string[]) => authors.join(', '),
//         },
//         {
//             title: 'Инвентарный номер',
//             dataIndex: 'inventoryNumber',
//             key: 'inventoryNumber',
//         },
//         {
//             title: 'Для выдачи',
//             dataIndex: 'loanable',
//             key: 'loanable',
//             render: (loanable: boolean) => (loanable ? 'Yes' : 'No'),
//         },
//         {
//             title: 'Выдано',
//             dataIndex: 'loaned',
//             key: 'loaned',
//             render: (loaned: boolean) => (loaned ? 'Yes' : 'No'),
//         },
//         {
//             title: 'Потеряно',
//             dataIndex: 'lost',
//             key: 'lost',
//             render: (lost: boolean) => (lost ? 'Yes' : 'No'),
//         },
//         {
//             title: 'Дата получения',
//             dataIndex: 'dateReceived',
//             key: 'dateReceived',
//             render: (text: string | null) => (text ? new Date(text).toLocaleDateString() : 'N/A'), // Format date
//         },
//         {
//             title: 'Дата спсиания',
//             dataIndex: 'dateWithdrawn',
//             key: 'dateWithdrawn',
//             render: (text: string | null) => (text ? new Date(text).toLocaleDateString() : 'N/A'), // Format date
//         },
//         {
//             title: 'Произведения',
//             dataIndex: 'publications',
//             key: 'publications',
//         },
//     ];

//     return (
//         <div>
//             <h1>Получить список инвентарных номеров и названий из библиотечного фонда, в 
//             которых содержится указанное произведение. </h1>
//             <Input 
//                 placeholder="Введите название произведения" 
//                 value={publication} 
//                 onChange={(e) => setPublication(e.target.value)} 
//                 style={{ width: 300, marginRight: 16 }} 
//             />
//             <Button type="primary" onClick={handleSearch}>
//                 Поиск
//             </Button>

//             {loading && <Spin style={{ marginTop: '16px' }} />}

//             {noResults && (
//                 <Alert
//                     message="No item copies found for the specified publication."
//                     type="info"
//                     showIcon
//                     style={{ marginTop: '16px', marginBottom: '16px' }}
//                 />
//             )}

//             <Table
//                 dataSource={itemCopies}
//                 columns={columns}
//                 rowKey="itemCopyId"
//                 pagination={{ pageSize: 10 }}
//                 locale={{ emptyText: loading ? 'Loading...' : 'Нет данныз' }}
//                 style={{ marginTop: '16px' }}
//             />
//         </div>
//     );
// };

// export default ItemCopiesByPublicationPage;
