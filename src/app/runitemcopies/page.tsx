"use client";
import React, { useEffect, useState } from "react";
import { Card, Button, Modal, Form, Input, Row, Col, message, Select, DatePicker } from "antd";
import axios from "axios";
import { ExclamationCircleOutlined } from "@ant-design/icons";
import { API_BASE_URL } from "../../../const";
import moment from "moment";

const { confirm } = Modal;

interface ItemCopy {
  id: string;
  itemId: string;
  shelfId: string;
  inventoryNumber: string;
  loanable: boolean;
  loaned: boolean;
  lost: boolean;
  dateReceived: string | null;
  dateWithdrawn: string | null;
}

const ItemCopiesPage: React.FC = () => {
  const [itemCopies, setItemCopies] = useState<ItemCopy[]>([]);
  const [isEditModalVisible, setIsEditModalVisible] = useState(false);
  const [isAddModalVisible, setIsAddModalVisible] = useState(false);
  const [editingItemCopy, setEditingItemCopy] = useState<ItemCopy | null>(null);
  const [form] = Form.useForm();

  const [shelves, setShelves] = useState([]);
    const [items, setItems] = useState([]);

    // Загрузка данных для селектов
    const fetchShelves = async () => {
    try {
        const response = await axios.get(`${API_BASE_URL}/api/Shelf`);
        setShelves(response.data);
    } catch (error) {
        console.error("Ошибка при загрузке полок", error);
    }
    };

    const fetchItems = async () => {
    try {
        const response = await axios.get(`${API_BASE_URL}/items`);
        setItems(response.data);
    } catch (error) {
        console.error("Ошибка при загрузке изданий", error);
    }
    };

    // Вызов функций при монтировании компонента
    useEffect(() => {
    fetchShelves();
    fetchItems();
    }, []);

  // Получение всех копий книг
  const fetchItemCopies = async () => {
    try {
      const response = await axios.get<ItemCopy[]>(`${API_BASE_URL}/itemcopies`);
      setItemCopies(response.data);
    } catch (error) {
      console.error("Ошибка при загрузке копий книг", error);
    } finally {
    }
  };

  useEffect(() => {
    fetchItemCopies();
  }, []);

  // Открытие модального окна для редактирования
  const openEditModal = (itemCopy: ItemCopy) => {
    setEditingItemCopy(itemCopy);
    setIsEditModalVisible(true);
  
    form.setFieldsValue({
      itemId: itemCopy.itemId,
      shelfId: itemCopy.shelfId,
      inventoryNumber: itemCopy.inventoryNumber,
      loanable: itemCopy.loanable,
      loaned: itemCopy.loaned,
      lost: itemCopy.lost,
      dateReceived: itemCopy.dateReceived ? moment(itemCopy.dateReceived) : null,
      dateWithdrawn: itemCopy.dateWithdrawn ? moment(itemCopy.dateWithdrawn) : null,
    });
  };

  // Закрытие модального окна
  const closeEditModal = () => {
    setEditingItemCopy(null);
    setIsEditModalVisible(false);
    form.resetFields();
  };

  // Открытие модального окна для добавления
  const openAddModal = () => {
    setIsAddModalVisible(true);
    form.resetFields();
  };

  // Закрытие модального окна для добавления
  const closeAddModal = () => {
    setIsAddModalVisible(false);
    form.resetFields();
  };

  // Добавление новой копии книги
  const addItemCopy = async (values: {itemId: string;shelfId:string;inventoryNumber:string;loanable:boolean;}) => {
    try {
      // Оставляем только необходимые поля для API
      const newItemCopy = {
        itemId: values.itemId,
        shelfId: values.shelfId,
        inventoryNumber: values.inventoryNumber,
        loanable: values.loanable ?? true, // Доступность по умолчанию true
      };
  
      const response = await axios.post(`${API_BASE_URL}/itemcopies`, newItemCopy);
      message.success("Копия книги успешно добавлена");
      closeAddModal();
      fetchItemCopies();
      console.log(response)
    } catch (error) {
      console.error("Ошибка при добавлении копии книги", error);
    }
  };

  // Обновление копии книги
  const updateItemCopy = async (values: {itemId:string;shelfId:string;inventoryNumber:string;loanable:string;loaned:string;lost:string;dateReceived: moment.Moment | null ;dateWithdrawn:moment.Moment | null;}) => {
    if (!editingItemCopy) return;
  
    try {
      const updatedItemCopy = {
        itemId: values.itemId,
        shelfId: values.shelfId,
        inventoryNumber: values.inventoryNumber,
        loanable: values.loanable,
        loaned: values.loaned,
        lost: values.lost,
        dateReceived: values.dateReceived ? values.dateReceived.toISOString() : null,
        dateWithdrawn: values.dateWithdrawn ? values.dateWithdrawn.toISOString() : null,
      };
  
      await axios.put(`${API_BASE_URL}/itemcopies/${editingItemCopy.id}`, updatedItemCopy);
      message.success("Копия книги успешно обновлена");
      closeEditModal();
      fetchItemCopies();
    } catch (error) {
      console.log("Ошибка при обновлении копии книги", error);
    }
  };
  

  // Удаление копии книги
  const confirmDelete = (id: string) => {
    confirm({
      title: "Вы уверены, что хотите удалить эту копию книги?",
      icon: <ExclamationCircleOutlined />,
      okText: "Да",
      cancelText: "Нет",
      onOk: async () => {
        try {
          await axios.delete(`${API_BASE_URL}/itemcopies/${id}`);
          message.success("Копия книги успешно удалена");
          fetchItemCopies();
        } catch (error) {
          console.log("Ошибка при обновлении копии книги", error);
        }
      },
    });
  };

  // Обработка отчета о потере
  const reportLost = async (id: string) => {
    try {
      await axios.post(`${API_BASE_URL}/itemcopies/${id}/report-lost`);
      message.success("Копия книги отмечена как потерянная");
      fetchItemCopies();
    } catch (error) {
      console.log("Ошибка при обновлении копии книги", error);
    }
  };

  // Отмена статуса потери
  const cancelLostStatus = async (id: string) => {
    try {
      await axios.post(`${API_BASE_URL}/itemcopies/${id}/cancel-lost`);
      message.success("Статус потери отменен");
      fetchItemCopies();
    } catch (error) {
      console.log("Ошибка при обновлении копии книги", error);
    }
  };

  return (
    <div style={{ padding: "20px" }}>
      <Button type="primary" style={{ marginBottom: "20px" }} onClick={openAddModal}>
        Добавить копию книги
      </Button>

      <Row gutter={[16, 16]}>
        {itemCopies.map((itemCopy) => (
          <Col key={itemCopy.id} xs={24} sm={12} md={8} lg={6}>
            <Card
              hoverable
              style={{
                maxWidth: "290px",
                margin: "0 auto",
                textAlign: "center",
              }}
            >
              <p>
                <b>Инвентарный номер:</b> {itemCopy.inventoryNumber}
              </p>
              <p>
                <b>Состояние:</b> {itemCopy.loanable ? "Доступна для аренды" : "Недоступна"}
              </p>
              <Button type="link" onClick={() => openEditModal(itemCopy)}>
                Редактировать
              </Button>
              <Button type="link" danger onClick={() => confirmDelete(itemCopy.id)}>
                Удалить
              </Button>
              <Button
                type="link"
                onClick={() => (itemCopy.lost ? cancelLostStatus(itemCopy.id) : reportLost(itemCopy.id))}
              >
                {itemCopy.lost ? "Отменить потерю" : "Отметить как потерянную"}
              </Button>
            </Card>
          </Col>
        ))}
      </Row>

      {/* Модальное окно для редактирования */}
      <Modal
        title="Редактировать копию книги"
        visible={isEditModalVisible}
        onCancel={closeEditModal}
        onOk={() => form.submit()}
        okText="Сохранить"
        cancelText="Отмена"
      >
        <Form form={form} layout="vertical" onFinish={updateItemCopy}>
            {/* Селект для полки */}
            <Form.Item
                name="shelfId"
                label="Полка"
                rules={[{ required: true, message: "Выберите полку" }]}
            >
                <Select placeholder="Выберите полку">
                {shelves.map((shelf : {id: string; number:string;}) => (
                    <Select.Option key={shelf.id} value={shelf.id}>
                    {`${shelf.number}`}
                    </Select.Option>
                ))}
                </Select>
            </Form.Item>

            {/* Селект для издания */}
            <Form.Item
                name="itemId"
                label="Издание"
                rules={[{ required: true, message: "Выберите издание" }]}
            >
                <Select placeholder="Выберите издание">
                {items.map((item: {id: string; title: string}) => (
                    <Select.Option key={item.id} value={item.id}>
                    {item.title}
                    </Select.Option>
                ))}
                </Select>
            </Form.Item>

            {/* Инвентарный номер */}
            <Form.Item
                name="inventoryNumber"
                label="Инвентарный номер"
                rules={[{ required: true, message: "Введите инвентарный номер" }]}
            >
                <Input />
            </Form.Item>

            {/* Флажки */}
            <Form.Item name="loanable" label="Доступна для аренды" valuePropName="checked">
                <Input type="checkbox" />
            </Form.Item>
            <Form.Item name="loaned" label="Арендована" valuePropName="checked">
                <Input type="checkbox" />
            </Form.Item>
            <Form.Item name="lost" label="Потеряна" valuePropName="checked">
                <Input type="checkbox" />
            </Form.Item>

            {/* Даты */}
            <Form.Item name="dateReceived" label="Дата получения">
                <DatePicker showTime format="YYYY-MM-DD HH:mm:ss" />
            </Form.Item>
            <Form.Item name="dateWithdrawn" label="Дата вывода">
                <DatePicker showTime format="YYYY-MM-DD HH:mm:ss" />
            </Form.Item>
            </Form>

      </Modal>

      {/* Модальное окно для добавления */}
      <Modal
        title="Добавить копию книги"
        visible={isAddModalVisible}
        onCancel={closeAddModal}
        onOk={() => form.submit()}
        okText="Добавить"
        cancelText="Отмена"
        >
        <Form form={form} layout="vertical" onFinish={addItemCopy}>
            {/* Селект для полки */}
            <Form.Item
            name="shelfId"
            label="Выберите полку"
            rules={[{ required: true, message: "Выберите полку" }]}
            >
            <Select placeholder="Выберите полку">
                {shelves.map((shelf: {id: string; number: string}) => (
                <Select.Option key={shelf.id} value={shelf.id}>
                    {`${shelf.number}`}
                </Select.Option>
                ))}
            </Select>
            </Form.Item>

            {/* Селект для издания */}
            <Form.Item
            name="itemId"
            label="Выберите издание"
            rules={[{ required: true, message: "Выберите издание" }]}
            >
            <Select placeholder="Выберите издание">
                {items.map((item: {id: string; title: string}) => (
                <Select.Option key={item.id} value={item.id}>
                    {item.title}
                </Select.Option>
                ))}
            </Select>
            </Form.Item>

            {/* Инвентарный номер */}
            <Form.Item
            name="inventoryNumber"
            label="Инвентарный номер"
            rules={[{ required: true, message: "Введите инвентарный номер" }]}
            >
            <Input />
            </Form.Item>

            {/* Флажок доступности */}
            <Form.Item
            name="loanable"
            label="Доступна для аренды"
            valuePropName="checked"
            >
            <Input type="checkbox" />
            </Form.Item>
        </Form>
        </Modal>


    </div>
  );
};

export default ItemCopiesPage;
