"use client";
import React, { useState } from 'react';
import axios from 'axios';
import { Form, DatePicker, Button, Table } from 'antd';

interface LibrarianWorkReport {
    librarianId: string;
    librarianName: string;
    numberOfServedReaders: number;
}

const LibrarianWorkReportPage: React.FC = () => {
    const [form] = Form.useForm();
    const [loading, setLoading] = useState(false);
    const [reports, setReports] = useState<LibrarianWorkReport[]>([]);

    const handleSubmit = async (values: any) => {
        const { startDate, endDate } = values;
        
        setLoading(true);
        setReports([]);

        try {
            const response = await axios.get<LibrarianWorkReport[]>('http://localhost:5251/api/Reader/librarianWorkReport9', {
                params: {
                    startDate: startDate.toISOString(),  
                    endDate: endDate.toISOString(),     
                },
            });
            setReports(response.data);
        } catch (error) {
            console.error('Error fetching librarian work reports', error);
        } finally {
            setLoading(false);
        }
    };

    const columns = [
        {
            title: 'ID библиотекаря',
            dataIndex: 'librarianId',
            key: 'librarianId',
        },
        {
            title: 'Имя',
            dataIndex: 'librarianName',
            key: 'librarianName',
        },
        {
            title: 'Количество обслуженных читателей',
            dataIndex: 'numberOfServedReaders',
            key: 'numberOfServedReaders',
        },
    ];

    return (
        <div>
            <h1>Получить данные о выработке библиотекарей (число обслуженных читателей в 
                указанный период времени).</h1>
            <Form 
                form={form} 
                layout="inline" 
                onFinish={handleSubmit}
            >
                <Form.Item 
                    name="startDate" 
                    label="Начальная дата" 
                    rules={[{ required: true, message: 'Please select a start date!' }]}
                >
                    <DatePicker />
                </Form.Item>
                <Form.Item 
                    name="endDate" 
                    label="Конченая дата" 
                    rules={[{ required: true, message: 'Please select an end date!' }]}
                >
                    <DatePicker />
                </Form.Item>
                <Form.Item>
                    <Button type="primary" htmlType="submit" loading={loading}>
                        Fetch Report
                    </Button>
                </Form.Item>
            </Form>

            <Table
                dataSource={reports}
                columns={columns}
                rowKey="librarianId"
                loading={loading}
                pagination={{ pageSize: 10 }}
                locale={{ emptyText: 'Нет записей' }}
            />
        </div>
    );
};
export default LibrarianWorkReportPage;
