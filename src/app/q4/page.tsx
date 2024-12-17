// "use client";

// import React, { useEffect, useState } from 'react';
// import axios from 'axios';
// import { Form, Button, Table, Select, Alert, DatePicker } from 'antd';
// import { NextPage } from 'next';
// import moment from 'moment';

// interface ReaderWithItemDto {
//     readerId: string;
//     readerFullName: string;
//     itemTitle: string;
//     issueDate: string;
// }

// interface ItemResponse {
//     id: string;
//     title: string;
// }

// const { Option } = Select;
// const { RangePicker } = DatePicker;

// const ReadersWithItemPage: NextPage = () => {
//     const [form] = Form.useForm();
//     const [loading, setLoading] = useState(false);
//     const [readers, setReaders] = useState<ReaderWithItemDto[]>([]);
//     const [items, setItems] = useState<ItemResponse[]>([]);
//     const [selectedItemId, setSelectedItemId] = useState<string | null>(null);
//     const [dateRange, setDateRange] = useState<[moment.Moment, moment.Moment] | null>(null);
//     const [noResults, setNoResults] = useState<boolean>(false);

//     useEffect(() => {
//         // Fetch items on load
//         const fetchItems = async () => {
//             try {
//                 const response = await axios.get<ItemResponse[]>('http://localhost:5251/Items');
//                 setItems(response.data);
//             } catch (error) {
//                 console.error('Error fetching items:', error);
//             }
//         };

//         fetchItems();
//     }, []);

//     const handleSearch = async () => {
//         if (!selectedItemId || !dateRange) return;
    
//         setLoading(true);
//         setReaders([]);
//         setNoResults(false);
    
//         const [startDate, endDate] = dateRange;
//         const formattedStartDate = startDate.toISOString(); 
//         const formattedEndDate = endDate.toISOString();
    
//         try {
//             const response = await axios.get<ReaderWithItemDto[]>(
//                 `http://localhost:5251/api/Reader/readersWithItemInDateRange4`, {
//                     params: {
//                         itemId: selectedItemId,
//                         startDate: formattedStartDate,
//                         endDate: formattedEndDate
//                     }
//                 });
            
//             setReaders(response.data);
//             setNoResults(response.data.length === 0);
//         } catch (error) {
//             console.error('Error fetching readers in date range:', error);
//             setNoResults(true);
//         } finally {
//             setLoading(false);
//         }
//     };
    

//     const columns = [
//         {
//             title: 'ID читателя',
//             dataIndex: 'readerId',
//             key: 'readerId',
//         },
//         {
//             title: 'Имя читателя',
//             dataIndex: 'readerFullName',
//             key: 'readerFullName',
//         },
//         {
//             title: 'Название издания',
//             dataIndex: 'itemTitle',
//             key: 'itemTitle',
//         },
//         {
//             title: 'Дата выдачи',
//             dataIndex: 'issueDate',
//             key: 'issueDate',
//             render: (date: string) => moment(date).format('DD-MM-YYYY'),
//         },
//     ];

//     return (
//         <div>
//             <h1>Получить перечень читателей, которые в течение указанного промежутка времени 
//                 получали издание с некоторым произведением, и название этого издания. 
//                 5. Выдать список изданий, которые в течение некоторого времени получал указан</h1>
//             <Form 
//                 form={form} 
//                 layout="vertical"
//                 initialValues={{ itemId: '', dateRange: [] }}
//             >
//                 <Form.Item name="itemId" label="Выберите издание">
//                     <Select 
//                         placeholder="Выберите издание"
//                         onChange={setSelectedItemId}
//                         allowClear
//                     >
//                         {items.map(item => (
//                             <Option key={item.id} value={item.id}>
//                                 {item.title}
//                             </Option>
//                         ))}
//                     </Select>
//                 </Form.Item>

//                 <Form.Item name="dateRange" label="Выберите дату">
//                     <RangePicker 
//                         onChange={(dates) => setDateRange(dates as [moment.Moment, moment.Moment])} 
//                         format="DD-MM-YYYY" 
//                     />
//                 </Form.Item>

//                 <Form.Item>
//                     <Button type="primary" onClick={handleSearch} loading={loading}>
//                         Поиск
//                     </Button>
//                 </Form.Item>
//             </Form>

//             {noResults && <Alert message="No readers found for the selected item and date range." type="info" showIcon style={{ marginBottom: '16px' }} />}

//             {!loading && readers.length === 0 && !noResults ? (
//                 <Alert message="No data available" type="info" showIcon style={{ marginBottom: '16px' }} />
//             ) : (
//                 <Table
//                     dataSource={readers}
//                     columns={columns}
//                     rowKey="readerId"
//                     loading={loading}
//                     pagination={{ pageSize: 10 }}
//                     locale={{ emptyText: 'Нет записей' }}
//                 />
//             )}
//         </div>
//     );
// };

// export default ReadersWithItemPage;
