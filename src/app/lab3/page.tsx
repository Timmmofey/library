"use client"

import React, { useState, useEffect } from "react";
import { Table, Button, Modal, Form, Input, Select, DatePicker, message } from "antd";
import axios from "axios";
import dayjs from "dayjs";

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

  useEffect(() => {
    fetchData();
    fetchSelectors();
  }, []);

  const fetchData = async () => {
    setLoading(true);
    try {
      const response = await axios.get<LoanRecord[]>("http://localhost:5251/api/Loan");
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
        axios.get<ItemCopy[]>("http://localhost:5251/ItemCopies"),
        axios.get<Reader[]>("http://localhost:5251/api/Reader"),
        axios.get<Librarian[]>("http://localhost:5251/Librarians"),
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
      await axios.delete(`http://localhost:5251/api/Loan/${id}`);
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
        `http://localhost:5251/api/Loan/${editingRecord?.id}`,
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
      await axios.post(`http://localhost:5251/api/Loan/${id}/return`, {}, {
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
      await axios.post(`http://localhost:5251/api/Loan/${id}/reportLost`, {}, {
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
        "http://localhost:5251/api/Loan",
        {
          itemCopyId: itemCopies.find((copy) => copy.inventoryNumber === values.inventoryNumber)?.id, 
          readerId: readers.find((reader) => reader.fullName === values.readerName)?.id, 
          dueDate: dayjs(values.dueDate).add(1, "day").startOf("day").toISOString(),
        },
        {
          withCredentials: true,
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
  
  const columns = [
    {
      title: "Инвентарный номер",
      dataIndex: "inventoryNumber",
      key: "inventoryNumber",
    },
    {
      title: "Читатель",
      dataIndex: "readerName",
      key: "readerName",
    },
    {
      title: "Библиотекарь",
      dataIndex: "librarianName",
      key: "librarianName",
    },
    {
      title: "Дата выдачи",
      dataIndex: "issueDate",
      key: "issueDate",

      render: (text : string) => dayjs(text).add(1, "day").format("YYYY-MM-DD")
    },
    {
      title: "Срок возврата",
      dataIndex: "dueDate",
      key: "dueDate",
      render: (text : string) => dayjs(text).format("YYYY-MM-DD"),
    },
    {
      title: "Дата возврата",
      dataIndex: "returnDate",
      key: "returnDate",
      render: (text : string) => (text ? dayjs(text).format("YYYY-MM-DD") : "—"),
    },
    {
      title: "Потеряна",
      dataIndex: "lost",
      key: "lost",
      render: (text : string) => (text ? "Да" : "Нет"),
    },
    {
        title: "Действия",
        key: "actions",
        render: (_ , record: LoanRecord) => (
          <>
            <Button onClick={() => editRecord(record)} type="link">
              Обновить
            </Button>
            <Button
              onClick={() => showConfirm("удалить", record, deleteRecord)}
              type="link"
              danger
            >
              Удалить
            </Button>
            { 
                !record.returnDate && 
                <Button
                onClick={() => showConfirm("принять возврат", record, acceptReturn)}
                type="link"
                >
                Принять возврат
                </Button>
            }
            { 
                !record.returnDate && !record.lost &&
                <Button
                onClick={() => showConfirm("сообщить о потере", record, reportLost)}
                type="link"
                danger
                >
                Сообщить о потере
                </Button>
            }
          </>
        ),
      }
      ,
  ];

  return (
    <>
        <Button
        type="primary"
        onClick={() => setAddModalVisible(true)} 
        >
        Выдать книгу
        </Button>


      <Table
        columns={columns}
        dataSource={data}
        rowKey="id"
        loading={loading}
      />

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
                <Select>
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
                <Select>
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
