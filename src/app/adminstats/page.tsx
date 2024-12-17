"use client";

import React, { useEffect, useState } from 'react';
import { Layout, Row, Col, Card, Spin, Typography } from 'antd';
import { Bar, Pie } from '@ant-design/charts';
import axios from 'axios';
import { API_BASE_URL } from "../../../const";

const { Header, Content, Footer } = Layout;
const { Text } = Typography;

const Dashboard: React.FC = () => {
  const [subscriptions, setSubscriptions] = useState([]);
  const [libraries, setLibraries] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const [subsResponse, libsResponse] = await Promise.all([
          axios.get(`${API_BASE_URL}/api/SubscriptionHistory`),
          axios.get(`${API_BASE_URL}/Library`),
        ]);

        setSubscriptions(subsResponse.data);
        setLibraries(libsResponse.data);
      } catch (error) {
        console.error('Error fetching data:', error);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, []);

  const libraryProfitData = libraries.map((library: any) => {
    const profit = subscriptions
      .filter((sub: any) => sub.libraryId === library.id)
      .reduce((sum: number, sub: any) => sum + sub.totalPrice, 0);

    return { libraryName: library.name, profit };
  });

  const totalProfitLibraries = libraryProfitData.reduce((acc, curr) => acc + curr.profit, 0);

  const monthlyProfitData = subscriptions.reduce((acc: Record<string, number>, sub: any) => {
    const month = new Date(sub.date).toLocaleString('default', { month: 'long' });
    acc[month] = (acc[month] || 0) + sub.totalPrice;
    return acc;
  }, {});

  const totalProfitMonthly = Object.values(monthlyProfitData).reduce((acc, curr) => acc + curr, 0);

  // Процент по библиотекам
  const libraryProfitPercentages = libraryProfitData.map((library) => {
    const percentage = ((library.profit / totalProfitLibraries) * 100).toFixed(2);
    return { ...library, percentage };
  });

  // Процент по месяцам
  const monthlyProfitPercentages = Object.entries(monthlyProfitData).map(([month, profit]) => {
    const percentage = ((profit / totalProfitMonthly) * 100).toFixed(2);
    return { month, profit, percentage };
  });

  // Конфигурация гистограммы (Bar)
  const barConfig = {
    data: libraryProfitData,
    xField: 'libraryName',
    yField: 'profit',
    seriesField: 'libraryName',
    color: '#5B8FF9',
    label: {
      position: 'middle',
      style: { fill: '#fff', fontSize: 14 },
    },
    xAxis: {
      title: { text: 'Library' },
    },
    yAxis: {
      title: { text: 'Profit ($)' },
      label: { formatter: (v: number) => `$${v}` },
    },
    meta: {
      profit: { alias: 'Profit ($)' },
    },
  };

  // Конфигурация круговой диаграммы (Pie)
  const pieConfig = {
    data: libraryProfitData,
    angleField: 'profit',
    colorField: 'libraryName',
    radius: 0.8,
    label: {
      type: 'outer',
      content: '{percentage}%',  // Выводим проценты
      style: { fontSize: 14, fill: '#fff' },  // Стиль для текста
    },
    legend: {
      position: 'bottom',
    },
    interactions: [
      {
        type: 'element-active',
      },
    ],
  };

  // Конфигурация гистограммы по месяцам (Monthly)
  const monthlyBarConfig = {
    data: Object.entries(monthlyProfitData).map(([month, profit]) => ({ month, profit })),
    xField: 'month',
    yField: 'profit',
    color: '#F4664A',
    label: {
      position: 'middle',
      style: { fill: '#FFFFFF', opacity: 0.6 },
    },
    xAxis: {
      title: { text: 'Month' },
      label: { rotate: 45 },
    },
    yAxis: {
      title: { text: 'Profit ($)' },
      label: { formatter: (v: number) => `$${v}` },
    },
    meta: {
      profit: { alias: 'Profit ($)' },
    },
  };

  return (
    <Layout>
      <Header style={{ color: 'white', textAlign: 'center', fontSize: '24px' }}>
        Статистика продаж абонемемнтов
      </Header>
      <Content style={{ padding: '16px' }}>
        {loading ? (
          <Spin size="large" />
        ) : (
          <Row gutter={[16, 16]}>
            <Col xs={24} sm={12} lg={12}>
              <Card title="Прибыль с продаж абонементов по бибилиотекам">
                <Bar {...barConfig} />
                <div style={{ textAlign: 'center', marginTop: '10px' }}>
                  {libraryProfitPercentages.map((library) => (
                    <Text key={library.libraryName}>
                      {library.libraryName}: {library.percentage}% <br />
                    </Text>
                  ))}
                </div>
              </Card>
            </Col>
            <Col xs={24} sm={12} lg={12}>
              <Card title="Процент прибыли библиотек">
                <Pie {...pieConfig} />
                <div style={{ textAlign: 'center', marginTop: '10px' }}>
                  {libraryProfitPercentages.map((library) => (
                    <Text key={library.libraryName}>
                      {library.libraryName}: {library.percentage}% <br />
                    </Text>
                  ))}
                </div>
              </Card>
            </Col>
            <Col xs={24}>
              <Card title="Прибыль по месяцам">
                <Bar {...monthlyBarConfig} />
                <div style={{ textAlign: 'center', marginTop: '10px' }}>
                  {monthlyProfitPercentages.map((monthData) => (
                    <Text key={monthData.month}>
                      {monthData.month}: {monthData.percentage}% <br />
                    </Text>
                  ))}
                </div>
              </Card>
            </Col>
          </Row>
        )}
      </Content>
      <Footer style={{ textAlign: 'center' }}>©2024 Library Analytics</Footer>
    </Layout>
  );
};

export default Dashboard;
