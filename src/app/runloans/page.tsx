"use client"

import React, { useState, useEffect } from "react";
import { Card, Space, Button, Modal, Form, Input, Select, DatePicker, message } from "antd";
import axios from "axios";
import dayjs from "dayjs";
import { useAuthStore } from '../../stores/authStore';
import Cookies from 'js-cookie';


import { API_BASE_URL } from '../../../const';


const { Option } = Select;

interface LoanRecord {
  id: number;
  inventoryNumber: string;
  readerName: string;
  librarianName: string;
  issueDate: string;
  dueDate: string;
  returnDate: string | null;
  lost: boolean;
}

interface ItemCopy {
  id: number;
  inventoryNumber: string;
  loaned: boolean;
  lost: boolean;
}

interface Reader {
  id: number;
  fullName: string;
}

interface Librarian {
  id: number;
  name: string;
}

const LoanTable = () => {
  const [data, setData] = useState<LoanRecord[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [editingRecord, setEditingRecord] = useState<LoanRecord | null>(null);
  const [modalVisible, setModalVisible] = useState<boolean>(false);
  const [addModalVisible, setAddModalVisible] = useState<boolean>(false);
  const [itemCopies, setItemCopies] = useState<ItemCopy[]>([]);
  const [readers, setReaders] = useState<Reader[]>([]);
  const [librarians, setLibrarians] = useState<Librarian[]>([]);

  const [searchInventoryNumber, setSearchInventoryNumber] = useState<string>('');
  const [searchReaderName, setSearchReaderName] = useState<string>('');

  const user = useAuthStore((state) => state.user);


  useEffect(() => {
    fetchData();
    fetchSelectors();
  }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const response = await axios.get<LoanRecord[]>(`${API_BASE_URL}/api/Loan`);
      setData(response.data);
    } catch (error) {
      console.error("Ошибка:", error);
      message.error("Ошибка загрузки данных");
    } finally {
      setLoading(false);
    }
  };

  const fetchSelectors = async () => {
    try {
      const [itemCopiesResponse, readersResponse, librariansResponse] = await Promise.all([
        axios.get<ItemCopy[]>(`${API_BASE_URL}/ItemCopies`),
        axios.get<Reader[]>(`${API_BASE_URL}/api/Reader`),
        axios.get<Librarian[]>(`${API_BASE_URL}/Librarians`),
      ]);
      setItemCopies(itemCopiesResponse.data);
      setReaders(readersResponse.data);
      setLibrarians(librariansResponse.data);
    } catch (error) {
      console.error("Ошибка:", error);
      message.error("Ошибка загрузки справочников");
    }
  };

  const deleteRecord = async (id: number) => {
    try {
      await axios.delete(`${API_BASE_URL}/api/Loan/${id}`);
      message.success("Запись удалена");
      fetchData();
      fetchSelectors();
    } catch (error) {
      console.error("Ошибка:", error);
      message.error("Ошибка удаления записи");
    }
  };

  const editRecord = (record: LoanRecord) => {
    setEditingRecord(record);
    setModalVisible(true);
  };

  const handleUpdate = async (values: LoanRecord) => {
    try {
      const toUtcDatePlusOneDay = (date) =>
        date ? dayjs(date).add(1, "day").startOf("day").toISOString() : null;

      const updatedRecord = {
        itemCopyId: itemCopies.find((copy) => copy.inventoryNumber === values.inventoryNumber)?.id, 
        readerId: readers.find((reader) => reader.fullName === values.readerName)?.id, 
        librarianId: librarians.find((librarian) => librarian.name === values.librarianName)?.id, 
        issueDate: toUtcDatePlusOneDay(values.issueDate), 
        dueDate: toUtcDatePlusOneDay(values.dueDate), 
        returnDate: toUtcDatePlusOneDay(values.returnDate), 
        lost: !!values.lost, 
      };

      console.log("Обновляемый объект с добавленным днем:", updatedRecord);

      await axios.put(
        `${API_BASE_URL}/api/Loan/${editingRecord?.id}`,
        updatedRecord
      );

      message.success("Запись обновлена");
      fetchData();
      fetchSelectors();
      setModalVisible(false);
    } catch (error) {
      console.error("Ошибка:", error);
      message.error("Ошибка обновления записи");
    }
  };

  const acceptReturn = async (id: number) => {
    try {
      await axios.post(`${API_BASE_URL}/api/Loan/${id}/return`, {}, {
        withCredentials: true,
      });
      message.success("Возврат успешно принят");
      fetchData(); 
      fetchSelectors();
    } catch (error) {
      console.error("Ошибка:", error);
      message.error("Ошибка при принятии возврата");
    }
  };

  // Сообщение о потере
  const reportLost = async (id: number) => {
    try {
      await axios.post(`${API_BASE_URL}/api/Loan/${id}/reportLost`, {}, {
        withCredentials: true,
      });
      message.success("Потеря книги зарегистрирована");
      fetchData(); 
      fetchSelectors();
    } catch (error) {
      console.error("Ошибка:", error);
      message.error("Ошибка при регистрации потери");
    }
  };

  const handleAdd = async (values: LoanRecord) => {
    try {
      await axios.post(
        `${API_BASE_URL}/api/Loan`,
        {
          itemCopyId: itemCopies.find((copy) => copy.inventoryNumber === values.inventoryNumber)?.id, 
          readerId: readers.find((reader) => reader.fullName === values.readerName)?.id, 
          dueDate: dayjs(values.dueDate).add(1, "day").startOf("day").toISOString(),
          //librarianId: user?.id
        },
        {
          headers: {
            Authorization: `Bearer ${Cookies.get('some-cookie')}`, // Добавляем токен из куки
          },
        }
      );
      message.success("Запись добавлена");
      fetchData();
      fetchSelectors();
      setAddModalVisible(false);
    } catch (error) {
      console.error("Ошибка:", error);
      message.error("Ошибка добавления записи");
    }
  };

  const showConfirm = (actionText: string, record: LoanRecord, actionHandler: (id: number) => void) => {
    Modal.confirm({
      title: `Вы действительно хотите ${actionText} издание ${record.inventoryNumber}?`,
      okText: "Да",
      cancelText: "Нет",
      onOk: () => actionHandler(record.id),
    });
  };

//   const filteredAndSortedData = data.filter((record) =>
//     record.inventoryNumber.toLowerCase().includes(searchInventoryNumber.toLowerCase()) &&
//     record.readerName.toLowerCase().includes(searchReaderName.toLowerCase())
//   ).sort((a, b) => {
//     if (!a.returnDate && b.returnDate) return -1; // Несданные книги выше
//     if (a.returnDate && !b.returnDate) return 1; 
//     return 0; // Сохраняем порядок для остальных
//   });

const filteredAndSortedData = data
.filter((record) =>
  record.inventoryNumber.toLowerCase().includes(searchInventoryNumber.toLowerCase()) &&
  record.readerName.toLowerCase().includes(searchReaderName.toLowerCase()) &&
  (!record.returnDate || record.lost) // Фильтруем только несданные или потерянные книги
)
.sort((a, b) => {
  if (!a.returnDate && b.returnDate) return -1; // Несданные книги выше
  if (a.returnDate && !b.returnDate) return 1; 
  return 0; // Сохраняем порядок для остальных
});


  return (
    <>
        <Button
        type="primary"
        onClick={() => setAddModalVisible(true)} 
        >
        Выдать книгу
        </Button>

      <Space direction="vertical" size="large" style={{ marginBottom: 16 }}>
        <Input
          placeholder="Поиск по инвентарному номеру"
          value={searchInventoryNumber}
          onChange={(e) => setSearchInventoryNumber(e.target.value)}
          style={{ width: 200 }}
        />
        <Input
          placeholder="Поиск по имени читателя"
          value={searchReaderName}
          onChange={(e) => setSearchReaderName(e.target.value)}
          style={{ width: 200 }}
        />
      </Space>

      <Space direction="vertical" size="large" style={{ width: "100%", justifyContent: "center" }}>
      {filteredAndSortedData.map((record) => (
  <Card
    key={record.id}
    title={`Инвентарный номер: ${record.inventoryNumber}`}
    style={{ width: 290, margin: "0 auto" }}
  >
    <p><strong>Читатель:</strong> {record.readerName}</p>
    <p><strong>Библиотекарь:</strong> {record.librarianName}</p>
    <p><strong>Дата выдачи:</strong> {dayjs(record.issueDate).add(1, "day").format("YYYY-MM-DD")}</p>
    <p><strong>Срок возврата:</strong> {dayjs(record.dueDate).format("YYYY-MM-DD")}</p>
    <p><strong>Дата возврата:</strong> {record.returnDate ? dayjs(record.returnDate).format("YYYY-MM-DD") : "—"}</p>
    <p><strong>Потеряна:</strong> {record.lost ? "Да" : "Нет"}</p>
    <div style={{ display: "flex", flexWrap: "wrap", gap: "8px", justifyContent: "center", marginTop: 16 }}>
      <Button type="link" style={{ flex: "1 1 auto", textAlign: "center" }} onClick={() => editRecord(record)}>
        Обновить
      </Button>
      <Button
        type="link"
        style={{ flex: "1 1 auto", textAlign: "center" }}
        danger
        onClick={() => showConfirm("удалить", record, deleteRecord)}
      >
        Удалить
      </Button>
      {!record.returnDate && (
        <Button
          type="link"
          style={{ flex: "1 1 auto", textAlign: "center" }}
          onClick={() => showConfirm("принять возврат", record, acceptReturn)}
        >
          Принять возврат
        </Button>
      )}
      {!record.returnDate && !record.lost && (
        <Button
          type="link"
          style={{ flex: "1 1 auto", textAlign: "center" }}
          danger
          onClick={() => showConfirm("сообщить о потере", record, reportLost)}
        >
          Сообщить о потере
        </Button>
      )}
    </div>
  </Card>
))}
      </Space>

        {modalVisible && (
          <Modal
            title="Редактировать запись"
            visible={modalVisible}
            onCancel={() => setModalVisible(false)}
            footer={null}
          >
            <Form
              initialValues={{
                ...editingRecord,
                issueDate: dayjs(editingRecord.issueDate),
                dueDate: dayjs(editingRecord.dueDate),
                returnDate: editingRecord.returnDate
                  ? dayjs(editingRecord.returnDate)
                  : null,
              }}
              onFinish={handleUpdate}
            >
              <Form.Item
                name="inventoryNumber"
                label="Инвентарный номер"
                rules={[{ required: true, message: "Инвентарный номер обязателен" }]}
              >
                <Input value={editingRecord.inventoryNumber} disabled />
              </Form.Item>
              <Form.Item
                name="readerName"
                label="Читатель"
                rules={[{ required: true, message: "Выберите читателя" }]}
              >
                <Select>
                  {readers.map((reader) => (
                    <Option key={reader.id} value={reader.fullName}>
                      {reader.fullName}
                    </Option>
                  ))}
                </Select>
              </Form.Item>
              <Form.Item
                name="librarianName"
                label="Библиотекарь"
                rules={[{ required: true, message: "Выберите библиотекаря" }]}
              >
                <Select>
                  {librarians.map((librarian) => (
                    <Option key={librarian.id} value={librarian.name}>
                      {librarian.name}
                    </Option>
                  ))}
                </Select>
              </Form.Item>
              <Form.Item
                name="issueDate"
                label="Дата выдачи"
                rules={[{ required: true, message: "Выберите дату выдачи" }]}
              >
                <DatePicker />
              </Form.Item>
              <Form.Item
                name="dueDate"
                label="Срок возврата"
                rules={[{ required: true, message: "Выберите срок возврата" }]}
              >
                <DatePicker />
              </Form.Item>
              <Form.Item name="returnDate" label="Дата возврата">
                <DatePicker />
              </Form.Item>
              <Form.Item name="lost" label="Потеряна">
                <Select disabled>
                  <Option value={null}>Нет</Option>
                  <Option value={false}>Нет</Option>
                  <Option value={true}>Да</Option>
                </Select>
              </Form.Item>
              <Form.Item>
                <Button type="primary" htmlType="submit">
                  Сохранить
                </Button>
              </Form.Item>
            </Form>
          </Modal>
        )}


        {addModalVisible && (
        <Modal
            title="Выдать книгу"
            visible={addModalVisible}
            onCancel={() => setAddModalVisible(false)} 
            footer={null} 
        >
            <Form onFinish={handleAdd}>
            <Form.Item
            name="inventoryNumber"
            label="Инвентарный номер"
            rules={[{ required: true, message: "Выберите инвентарный номер" }]}
            >
            <Select
                showSearch
                filterOption={(input, option) =>
                option?.children?.toLowerCase().includes(input.toLowerCase())
                }
            >
                {itemCopies
                .filter(copy => !copy.loaned && !copy.lost)
                .map((copy) => (
                    <Option key={copy.id} value={copy.inventoryNumber}>
                    {copy.inventoryNumber}
                    </Option>
                ))}
            </Select>
            </Form.Item>

            <Form.Item
            name="readerName"
            label="Читатель"
            rules={[{ required: true, message: "Выберите читателя" }]}
            >
            <Select
                showSearch
                filterOption={(input, option) =>
                option?.children?.toLowerCase().includes(input.toLowerCase())
                }
            >
                {readers.map((reader) => (
                <Option key={reader.id} value={reader.fullName}>
                    {reader.fullName}
                </Option>
                ))}
            </Select>
            </Form.Item>


            <Form.Item
                name="dueDate"
                label="Срок возврата"
                rules={[{ required: true, message: "Выберите срок возврата" }]}
            >
                <DatePicker />
            </Form.Item>

            <Form.Item>
                <Button type="primary" htmlType="submit">
                Добавить
                </Button>
            </Form.Item>
            </Form>
        </Modal>
        )}

    </>
  );
};

export default LoanTable;
