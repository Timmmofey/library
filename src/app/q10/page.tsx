// "use client";
// import React, { useState } from 'react';
// import axios from 'axios';
// import { Table, Button, Alert } from 'antd';

// interface ReaderDto1 {
//     id: string;
//     email: string;
//     fullName: string;
//     libraryName: string | null;
//     readerCategoryId: string | null;
//     subscriptionEndDate: string | null;
//     educationalInstitution: string | null;
//     faculty: string | null;
//     course: string | null;
//     groupNumber: string | null;
//     organization: string | null;
//     researchTopic: string | null;
// }

// const OverdueReadersPage: React.FC = () => {
//     const [loading, setLoading] = useState(false);
//     const [readers, setReaders] = useState<ReaderDto1[]>([]);
//     const [noResults, setNoResults] = useState(false);

//     const fetchOverdueReaders = async () => {
//         setLoading(true);
//         setNoResults(false);
//         setReaders([]);

//         try {
//             const response = await axios.get<ReaderDto1[]>('http://localhost:5251/api/Reader/readersWithOverdueLoans10');
//             setReaders(response.data);
//             setNoResults(response.data.length === 0); 
//         } catch (error) {
//             console.error('Error fetching overdue readers:', error);
//             setNoResults(true);
//         } finally {
//             setLoading(false);
//         }
//     };

//     const columns = [
//         {
//             title: 'Имя',
//             dataIndex: 'fullName',
//             key: 'fullName',
//         },
//         {
//             title: 'Email',
//             dataIndex: 'email',
//             key: 'email',
//         },
//         {
//             title: 'Дата окончания подписки',
//             dataIndex: 'subscriptionEndDate',
//             key: 'subscriptionEndDate',
//             render: (date: string | null) => date ? new Date(date).toLocaleDateString() : 'N/A',
//         },
//         {
//             title: 'Образовательное учреждение',
//             dataIndex: 'educationalInstitution',
//             key: 'educationalInstitution',
//         },
//         {
//             title: 'Факультет',
//             dataIndex: 'faculty',
//             key: 'faculty',
//         },
//         {
//             title: 'Курс',
//             dataIndex: 'course',
//             key: 'course',
//         },
//         {
//             title: 'Номер группы',
//             dataIndex: 'groupNumber',
//             key: 'groupNumber',
//         },
//         {
//             title: 'Организация',
//             dataIndex: 'organization',
//             key: 'organization',
//         },
//         {
//             title: 'Тема иследованичв',
//             dataIndex: 'researchTopic',
//             key: 'researchTopic',
//         },
//     ];

//     return (
//         <div>
//             <h1>Получить список читателей с просроченным сроком литературы. 
//             </h1>
//             <Button type="primary" onClick={fetchOverdueReaders} loading={loading}>
//                 Получить
//             </Button>

//             {noResults && (
//                 <Alert 
//                     message="No readers with overdue loans found." 
//                     type="info" 
//                     showIcon 
//                     style={{ marginTop: '16px', marginBottom: '16px' }} 
//                 />
//             )}

//             <Table
//                 dataSource={readers}
//                 columns={columns}
//                 rowKey="id"
//                 loading={loading}
//                 pagination={{ pageSize: 10 }}
//                 locale={{ emptyText: 'Нет записей' }}
//             />
//         </div>
//     );
// };

// export default OverdueReadersPage;
